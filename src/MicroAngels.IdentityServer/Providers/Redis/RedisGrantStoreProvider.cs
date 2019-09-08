using IdentityServer4.Models;
using MicroAngels.Logger;
using Microsoft.AspNetCore.Authentication;
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
		public RedisOperationalStoreOptions(string conn)
		{
			RedisConnectionString = conn;
		}
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

		private readonly ILogger logger;

		private ISystemClock clock;

		public RedisGrantStoreProvider(RedisOperationalStoreOptions optons, ILogger logger)
		{
			var multiplexer = new RedisMultiplexer<RedisOperationalStoreOptions>(optons);
			options = multiplexer.RedisOptions;
			database = multiplexer.Database;
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			//this.clock = clock;
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
				var expiresIn = new TimeSpan(0,0,50);
				if (!string.IsNullOrEmpty(grant.SubjectId))
				{
					var setKey = GetSetKey(grant.SubjectId, grant.ClientId, grant.Type);
					var setKeyforSubject = GetSetKey(grant.SubjectId);
					var setKeyforClient = GetSetKey(grant.SubjectId, grant.ClientId);

					var ttlOfClientSet = this.database.KeyTimeToLiveAsync(setKeyforClient);
					var ttlOfSubjectSet = this.database.KeyTimeToLiveAsync(setKeyforSubject);

					await Task.WhenAll(ttlOfSubjectSet, ttlOfClientSet).ConfigureAwait(false);
					await database.StringSetAsync(grantKey, data, expiresIn);
					await database.StringSetAsync(setKeyforSubject, grantKey);
					await database.StringSetAsync(setKeyforClient, grantKey);
					await database.StringSetAsync(setKey, grantKey);
					//if ((ttlOfSubjectSet.Result ?? TimeSpan.Zero) <= expiresIn)
					//	await database.KeyExpireAsync(setKeyforSubject, expiresIn);
					//if ((ttlOfClientSet.Result ?? TimeSpan.Zero) <= expiresIn)
					//	await database.KeyExpireAsync(setKeyforClient, expiresIn);

					//await database.KeyExpireAsync(setKey, expiresIn);
					
					//var transaction = this.database.CreateTransaction();
					//await transaction.StringSetAsync(grantKey, data, expiresIn);
					//await transaction.SetAddAsync(setKeyforSubject, grantKey);
					//await transaction.SetAddAsync(setKeyforClient, grantKey);
					//await transaction.SetAddAsync(setKey, grantKey);
					//if ((ttlOfSubjectSet.Result ?? TimeSpan.Zero) <= expiresIn)
					//	await transaction.KeyExpireAsync(setKeyforSubject, expiresIn);
					//if ((ttlOfClientSet.Result ?? TimeSpan.Zero) <= expiresIn)
					//	await transaction.KeyExpireAsync(setKeyforClient, expiresIn);
					//await transaction.KeyExpireAsync(setKey, expiresIn);
					//await transaction.ExecuteAsync().ConfigureAwait(false);
				}
				else
				{
					await this.database.StringSetAsync(grantKey, data, expiresIn).ConfigureAwait(false);
				}
				logger.Info($"grant for subject {grant.SubjectId}, clientId {grant.ClientId}, grantType {grant.Type} persisted successfully", null);
			}
			catch (Exception ex)
			{
				logger.Error($"exception storing persisted grant to Redis database for subject {grant.SubjectId}, clientId {grant.ClientId}, grantType {grant.Type} : {ex.Message}", null);
				throw ex;
			}
		}

		public async Task<PersistedGrant> GetAsync(string key)
		{
			var data = await this.database.StringGetAsync(GetKey(key)).ConfigureAwait(false);
			logger.Info($"{key} found in database: {data.HasValue}", null);
			return data.HasValue ? ConvertFromJson(data) : null;
		}

		public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
		{
			var setKey = GetSetKey(subjectId);
			var (grants, keysToDelete) = await GetGrants(setKey).ConfigureAwait(false);
			if (keysToDelete.Any())
				await this.database.SetRemoveAsync(setKey, keysToDelete.ToArray()).ConfigureAwait(false);
			logger.Info($"{grants.Count()} persisted grants found for {subjectId}", null);
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
					logger.Info($"no {key} persisted grant found in database");
					return;
				}
				var grantKey = GetKey(key);
				logger.Info($"removing {key} persisted grant from database");
				var transaction = this.database.CreateTransaction();
				await transaction.KeyDeleteAsync(grantKey);
				await transaction.SetRemoveAsync(GetSetKey(grant.SubjectId), grantKey);
				await transaction.SetRemoveAsync(GetSetKey(grant.SubjectId, grant.ClientId), grantKey);
				await transaction.SetRemoveAsync(GetSetKey(grant.SubjectId, grant.ClientId, grant.Type), grantKey);
				await transaction.ExecuteAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.Error($"exception removing {key} persisted grant from database: {ex.Message}");
				throw ex;
			}

		}

		public async Task RemoveAllAsync(string subjectId, string clientId)
		{
			try
			{
				var setKey = GetSetKey(subjectId, clientId);
				var grantsKeys = await this.database.SetMembersAsync(setKey).ConfigureAwait(false);
				logger.Info($"removing {grantsKeys.Count()} persisted grants from database for subject {subjectId}, clientId {clientId}");
				if (!grantsKeys.Any()) return;
				var transaction = this.database.CreateTransaction();
				await transaction.KeyDeleteAsync(grantsKeys.Select(_ => (RedisKey)_.ToString()).Concat(new RedisKey[] { setKey }).ToArray());
				await transaction.SetRemoveAsync(GetSetKey(subjectId), grantsKeys);
				await transaction.ExecuteAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.Error($"exception removing persisted grants from database for subject {subjectId}, clientId {clientId}: {ex.Message}");
				throw ex;
			}
		}

		public async Task RemoveAllAsync(string subjectId, string clientId, string type)
		{
			try
			{
				var setKey = GetSetKey(subjectId, clientId, type);
				var grantsKeys = await this.database.SetMembersAsync(setKey).ConfigureAwait(false);
				logger.Info($"removing {grantsKeys.Count()} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {type}");
				if (!grantsKeys.Any()) return;
				var transaction = this.database.CreateTransaction();
				await transaction.KeyDeleteAsync(grantsKeys.Select(_ => (RedisKey)_.ToString()).Concat(new RedisKey[] { setKey }).ToArray());
				await transaction.SetRemoveAsync(GetSetKey(subjectId, clientId), grantsKeys);
				await transaction.SetRemoveAsync(GetSetKey(subjectId), grantsKeys);
				await transaction.ExecuteAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.Error($"exception removing persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {type}: {ex.Message}");
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
