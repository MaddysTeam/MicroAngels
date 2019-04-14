using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Providers
{

    public interface IResourceProvider
    {
        Task<ApiResource> FindResource(string resourceName);

        Task<IEnumerable<ApiResource>> FindResourcesByScopes(IEnumerable<string> scopes);
    }

}
