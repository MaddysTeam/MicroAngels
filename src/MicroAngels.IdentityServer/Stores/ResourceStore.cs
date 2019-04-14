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
            this._resourceProvider = resourceProvider;
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        Task<IdentityServer4.Models.Resources> IResourceStore.GetAllResourcesAsync()
        {
            throw new NotImplementedException();
        }

        private readonly IResourceProvider _resourceProvider;

    }

}
