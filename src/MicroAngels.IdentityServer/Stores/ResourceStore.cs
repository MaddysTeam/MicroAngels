using IdentityServer4.Models;
using IdentityServer4.Stores;
using MicroAngels.IdentityServer.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer
{

    public class ResourceStore : IResourceStore
    {

        public ResourceStore(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
           return await _resourceProvider.FindResource(name);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await _resourceProvider.FindResourcesByScopes(scopeNames);
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await _resourceProvider.FindIdentityResourcesByScopeAsync(scopeNames);
        }

        public async Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            return await _resourceProvider.GetAllResourcesAsync();
        }

        private readonly IResourceProvider _resourceProvider;

    }

}
