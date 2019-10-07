using IdentityServer4.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MicroAngels.IdentityServer.Models;
using MySql.Data.MySqlClient;

namespace MicroAngels.IdentityServer.Providers.MySql
{

    public class MySqlClientProvider : IClientProvider
    {

        public MySqlClientProvider(MySqlStoreOptions storeOptions)
        {
            _storeOptions = storeOptions;
        }

        public async Task<Client> FindClientById(string clientId)
        {
            var _client = new Client();
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				var query = await connection.QueryMultipleAsync(Sqls.Client, new { client = clientId });
                var clients = query.Read<IdentityClient>();
                var clientSecrets = query.Read<IdentityClientSecret>();
                var clientGrantTypes = query.Read<IdentityClientGrantType>();
                var clientScopes = query.Read<IdentityClientScope>();

                if(clients!=null && clients.AsList().Count > 0)
                {
                    var client = clients.AsList()[0];
                    client.ClientSecrets = clientSecrets.AsList();
                    client.AllowedGrantTypes = clientGrantTypes.AsList();
                    client.AllowedScopes = clientScopes.AsList();

                    _client = client.Map();
                }
            }

            return _client;
               
        }

        private MySqlStoreOptions _storeOptions;
    }

}
