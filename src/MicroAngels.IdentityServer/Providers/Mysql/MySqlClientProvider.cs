using IdentityServer4.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;


namespace MicroAngels.IdentityServer.Providers.MySql
{

    class MySqlClientProvider : IClientProvider
    {

        public async Task<Client> FindClientById(string clientId)
        {
            using (var connection = new SqlConnection(_storeOptions.ConnectionStrings))
            {
                var query = await connection.QueryMultipleAsync(_clientSql, new { client = clientId });
                var client = query.Read<IdentityClient>();
                var clientGrantTypes = query.Read<IdentityGrantType>();
                var clientScopes = query.Read<IdentityScope>();
                var clientSecrets = query.Read<IdentitySecrets>();

                //TODO: return MAP to Client
            }
                

            throw new NotImplementedException();
        }


        private MySqlStoreOptions _storeOptions;
        private string _clientSql = @"select * from Clients where ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientGrantTypes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientRedirectUris t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientScopes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientSecrets t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;";

    }

}
