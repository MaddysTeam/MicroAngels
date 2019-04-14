using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Providers
{

    public interface IClientProvider
    {
        Task<Client> FindClientById(string clientId);
    }

}
