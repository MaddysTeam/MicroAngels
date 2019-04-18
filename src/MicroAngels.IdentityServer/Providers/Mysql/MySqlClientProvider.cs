using IdentityServer4.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MicroAngels.IdentityServer.Models;
using MySql.Data.MySqlClient;

namespace MicroAngels.IdentityServer.Providers.MySql
{

    class MySqlClientProvider : IClientProvider
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
                var query = await connection.QueryMultipleAsync(_clientSql, new { client = clientId });
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
        private string _clientSql = @"select * from Clients where ClientId=@client and Enabled=1;
               select t3.* from Clients t1 inner join ClientSecrets t3 on t1.Id=t3.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientGrantTypes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientScopes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientRedirectUris t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               ";

    }

}
