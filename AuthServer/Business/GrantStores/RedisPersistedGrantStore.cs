using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity
{

    public class RedisPersistedGrantStore : IPersistedGrantStore
    {
        static Dictionary<string, PersistedGrant> _dic;
        private readonly IConfiguration _configuration;

        public RedisPersistedGrantStore(IConfiguration configuration)
        {
            _configuration = configuration;
            _dic = new Dictionary<string, PersistedGrant>();
        }

        /// <summary>Stores the grant.</summary>
        /// <param name="grant">The grant.</param>
        /// <returns></returns>
        public async Task StoreAsync(PersistedGrant grant)
        {
            //_dic.Add(grant.Key, grant);
            //var accessTokenLifetime = double.Parse(_configuration.GetConnectionString("accessTokenLifetime"));
            //var timeSpan = TimeSpan.FromSeconds(accessTokenLifetime);
            //_cacheClient?.SetAsync(grant.Key, grant, timeSpan);
            //return Task.CompletedTask;
        }

        /// <summary>Gets the grant.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<PersistedGrant> GetAsync(string key)
        {
            return null;
           // return _dic[key];
            //if (_cacheClient.ExistsAsync(key).Result)
            //{
            //    var ss = _cacheClient.GetAsync<PersistedGrant>(key).Result;
            //    return Task.FromResult<PersistedGrant>(_cacheClient.GetAsync<PersistedGrant>(key).Result.Value);
            //}
            //return Task.FromResult<PersistedGrant>((PersistedGrant)null);
        }

        /// <summary>Gets all grants for a given subject id.</summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return null;
        }

        /// <summary>Removes the grant by key.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {

        }

        /// <summary>
        /// Removes all grants for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
        }

        /// <summary>
        /// Removes all grants of a give type for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {

        }

    }

}
