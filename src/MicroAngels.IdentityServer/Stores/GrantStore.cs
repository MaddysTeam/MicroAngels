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

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId) => await _provider.GetAllAsync(subjectId);

        public async Task<PersistedGrant> GetAsync(string key) => await _provider.GetAsync(key);

        public async Task RemoveAllAsync(string subjectId, string clientId) => await _provider.RemoveAllAsync(subjectId, clientId);

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)=> await _provider.RemoveAllAsync(subjectId, clientId,type);

        public async Task RemoveAsync(string key) => await _provider.RemoveAsync(key);

        public async Task StoreAsync(PersistedGrant grant) => await _provider.StoreAsync(grant);

    }

}
