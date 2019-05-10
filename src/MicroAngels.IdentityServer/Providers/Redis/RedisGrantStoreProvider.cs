﻿using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MicroAngels.IdentityServer.Providers.Redis
{

    public abstract class RedisOptions
    {
        /// <summary>
        ///Configuration options objects for StackExchange.Redis Library.
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }

        /// <summary>
        /// Connection String for connecting to Redis Instance.
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        ///The specific Db number to connect to, default is -1.
        /// </summary>
        public int Db { get; set; } = -1;

        private string _keyPrefix = string.Empty;

        /// <summary>
        /// The Prefix to add to each key stored on Redis Cache, default is Empty.
        /// </summary>
        public string KeyPrefix
        {
            get
            {
                return string.IsNullOrEmpty(this._keyPrefix) ? this._keyPrefix : $"{_keyPrefix}:";
            }
            set
            {
                this._keyPrefix = value;
            }
        }

        internal RedisOptions()
        {
            this.multiplexer = GetConnectionMultiplexer();
        }

        private Lazy<IConnectionMultiplexer> GetConnectionMultiplexer()
        {
            return new Lazy<IConnectionMultiplexer>(
                () => string.IsNullOrEmpty(this.RedisConnectionString)
                    ? ConnectionMultiplexer.Connect(this.ConfigurationOptions)
                    : ConnectionMultiplexer.Connect(this.RedisConnectionString));
        }

        private Lazy<IConnectionMultiplexer> multiplexer = null;

        internal IConnectionMultiplexer Multiplexer => this.multiplexer.Value;
    }

    /// <summary>
    /// Represents Redis Operational store options.
    /// </summary>
    public class RedisOperationalStoreOptions : RedisOptions
    {

    }

    /// <summary>
    /// Represents Redis Cache options.
    /// </summary>
    public class RedisCacheOptions : RedisOptions
    {

    }

    /// <summary>
    /// represents Redis general multiplexer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisMultiplexer<T> where T : RedisOptions
    {
        public RedisMultiplexer(T redisOptions)
        {
            this.RedisOptions = redisOptions;
            this.GetDatabase();
        }

        private void GetDatabase()
        {
            this.Database = this.RedisOptions.Multiplexer.GetDatabase(string.IsNullOrEmpty(this.RedisOptions.RedisConnectionString) ? -1 : this.RedisOptions.Db);
        }

        internal T RedisOptions { get; }

        internal IDatabase Database { get; private set; }
    }

    /// <summary>
    /// Redis grant store persist provider
    /// </summary>
    public class RedisGrantStoreProvider : IGrantStoreProvider
    {

        private readonly RedisOperationalStoreOptions options;

        private readonly IDatabase database;

        private readonly ILogger<RedisGrantStoreProvider> logger;

        private ISystemClock clock;

        public RedisGrantStoreProvider(RedisMultiplexer<RedisOperationalStoreOptions> multiplexer, ILogger<RedisGrantStoreProvider> logger, ISystemClock clock)
        {
            if (multiplexer is null)
                throw new ArgumentNullException(nameof(multiplexer));
            options = multiplexer.RedisOptions;
            database = multiplexer.Database;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.clock = clock;
        }

        private string GetKey(string key) => $"{this.options.KeyPrefix}{key}";

        private string GetSetKey(string subjectId) => $"{this.options.KeyPrefix}{subjectId}";

        private string GetSetKey(string subjectId, string clientId) => $"{this.options.KeyPrefix}{subjectId}:{clientId}";

        private string GetSetKey(string subjectId, string clientId, string type) => $"{this.options.KeyPrefix}{subjectId}:{clientId}:{type}";

        public async Task StoreAsync(PersistedGrant grant)
        {
            if (grant == null)
                throw new ArgumentNullException(nameof(grant));
            try
            {
                var data = ConvertToJson(grant);
                var grantKey = GetKey(grant.Key);
                var expiresIn = grant.Expiration - this.clock.UtcNow;
                if (!string.IsNullOrEmpty(grant.SubjectId))
                {
                    var setKey = GetSetKey(grant.SubjectId, grant.ClientId, grant.Type);
                    var setKeyforSubject = GetSetKey(grant.SubjectId);
                    var setKeyforClient = GetSetKey(grant.SubjectId, grant.ClientId);

                    var ttlOfClientSet = this.database.KeyTimeToLiveAsync(setKeyforClient);
                    var ttlOfSubjectSet = this.database.KeyTimeToLiveAsync(setKeyforSubject);

                    await Task.WhenAll(ttlOfSubjectSet, ttlOfClientSet).ConfigureAwait(false);

                    var transaction = this.database.CreateTransaction();
                    transaction.StringSetAsync(grantKey, data, expiresIn);
                    transaction.SetAddAsync(setKeyforSubject, grantKey);
                    transaction.SetAddAsync(setKeyforClient, grantKey);
                    transaction.SetAddAsync(setKey, grantKey);
                    if ((ttlOfSubjectSet.Result ?? TimeSpan.Zero) <= expiresIn)
                        transaction.KeyExpireAsync(setKeyforSubject, expiresIn);
                    if ((ttlOfClientSet.Result ?? TimeSpan.Zero) <= expiresIn)
                        transaction.KeyExpireAsync(setKeyforClient, expiresIn);
                    transaction.KeyExpireAsync(setKey, expiresIn);
                    await transaction.ExecuteAsync().ConfigureAwait(false);
                }
                else
                {
                    await this.database.StringSetAsync(grantKey, data, expiresIn).ConfigureAwait(false);
                }
                logger.LogDebug($"grant for subject {grant.SubjectId}, clientId {grant.ClientId}, grantType {grant.Type} persisted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"exception storing persisted grant to Redis database for subject {grant.SubjectId}, clientId {grant.ClientId}, grantType {grant.Type} : {ex.Message}");
                throw ex;
            }
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var data = await this.database.StringGetAsync(GetKey(key)).ConfigureAwait(false);
            logger.LogDebug($"{key} found in database: {data.HasValue}");
            return data.HasValue ? ConvertFromJson(data) : null;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var setKey = GetSetKey(subjectId);
            var (grants, keysToDelete) = await GetGrants(setKey).ConfigureAwait(false);
            if (keysToDelete.Any())
                await this.database.SetRemoveAsync(setKey, keysToDelete.ToArray()).ConfigureAwait(false);
            logger.LogDebug($"{grants.Count()} persisted grants found for {subjectId}");
            return grants.Where(_ => _.HasValue).Select(_ => ConvertFromJson(_));
        }

        private async Task<(IEnumerable<RedisValue> grants, IEnumerable<RedisValue> keysToDelete)> GetGrants(string setKey)
        {
            var grantsKeys = await this.database.SetMembersAsync(setKey).ConfigureAwait(false);
            if (!grantsKeys.Any())
                return (Enumerable.Empty<RedisValue>(), Enumerable.Empty<RedisValue>());
            var grants = await this.database.StringGetAsync(grantsKeys.Select(_ => (RedisKey)_.ToString()).ToArray()).ConfigureAwait(false);
            var keysToDelete = grantsKeys.Zip(grants, (key, value) => new KeyValuePair<RedisValue, RedisValue>(key, value))
                                         .Where(_ => !_.Value.HasValue).Select(_ => _.Key);
            return (grants, keysToDelete);
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                var grant = await this.GetAsync(key).ConfigureAwait(false);
                if (grant == null)
                {
                    logger.LogDebug($"no {key} persisted grant found in database");
                    return;
                }
                var grantKey = GetKey(key);
                logger.LogDebug($"removing {key} persisted grant from database");
                var transaction = this.database.CreateTransaction();
                transaction.KeyDeleteAsync(grantKey);
                transaction.SetRemoveAsync(GetSetKey(grant.SubjectId), grantKey);
                transaction.SetRemoveAsync(GetSetKey(grant.SubjectId, grant.ClientId), grantKey);
                transaction.SetRemoveAsync(GetSetKey(grant.SubjectId, grant.ClientId, grant.Type), grantKey);
                await transaction.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError($"exception removing {key} persisted grant from database: {ex.Message}");
                throw ex;
            }

        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            try
            {
                var setKey = GetSetKey(subjectId, clientId);
                var grantsKeys = await this.database.SetMembersAsync(setKey).ConfigureAwait(false);
                logger.LogDebug($"removing {grantsKeys.Count()} persisted grants from database for subject {subjectId}, clientId {clientId}");
                if (!grantsKeys.Any()) return;
                var transaction = this.database.CreateTransaction();
                transaction.KeyDeleteAsync(grantsKeys.Select(_ => (RedisKey)_.ToString()).Concat(new RedisKey[] { setKey }).ToArray());
                transaction.SetRemoveAsync(GetSetKey(subjectId), grantsKeys);
                await transaction.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError($"exception removing persisted grants from database for subject {subjectId}, clientId {clientId}: {ex.Message}");
                throw ex;
            }
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            try
            {
                var setKey = GetSetKey(subjectId, clientId, type);
                var grantsKeys = await this.database.SetMembersAsync(setKey).ConfigureAwait(false);
                logger.LogDebug($"removing {grantsKeys.Count()} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {type}");
                if (!grantsKeys.Any()) return;
                var transaction = this.database.CreateTransaction();
                transaction.KeyDeleteAsync(grantsKeys.Select(_ => (RedisKey)_.ToString()).Concat(new RedisKey[] { setKey }).ToArray());
                transaction.SetRemoveAsync(GetSetKey(subjectId, clientId), grantsKeys);
                transaction.SetRemoveAsync(GetSetKey(subjectId), grantsKeys);
                await transaction.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError($"exception removing persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {type}: {ex.Message}");
                throw ex;
            }
        }

        #region Json
        private static string ConvertToJson(PersistedGrant grant)
        {
            return JsonConvert.SerializeObject(grant);
        }

        private static PersistedGrant ConvertFromJson(string data)
        {
            return JsonConvert.DeserializeObject<PersistedGrant>(data);
        }

        #endregion
    }

}