using IdentityServer4.Models;
using IdentityServer4.Stores;
using MicroAngels.IdentityServer.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer
{

    public class GrantStore : IPersistedGrantStore
    {

        private readonly IGrantStoreProvider _provider;

        public GrantStore(IGrantStoreProvider provider)
        {
            _provider = provider;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId) => _provider.GetAllAsync(subjectId);

        public Task<PersistedGrant> GetAsync(string key) => _provider.GetAsync(key);

        public Task RemoveAllAsync(string subjectId, string clientId) => _provider.RemoveAllAsync(subjectId, clientId);

        public Task RemoveAllAsync(string subjectId, string clientId, string type)=>_provider.RemoveAllAsync(subjectId, clientId,type);

        public Task RemoveAsync(string key) => _provider.RemoveAsync(key);

        public Task StoreAsync(PersistedGrant grant) => _provider.StoreAsync(grant);

    }

}
