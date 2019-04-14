using IdentityServer4.Models;
using IdentityServer4.Stores;
using MicroAngels.IdentityServer.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer
{

    public class ClientStore : IClientStore
    {

        public ClientStore(IClientProvider provider)
        {
            _clientProvider = provider;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
           return await _clientProvider.FindClientById(clientId);
        }

        private readonly IClientProvider _clientProvider;
    }

}
