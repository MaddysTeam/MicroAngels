using IdentityServer4.Models;
using MicroAngels.IdentityServer.Providers.Mysql;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MicroAngels.IdentityServer.Providers.MySql
{

    public class MySqlResourceProvider : IResourceProvider
    {

        public MySqlResourceProvider(ILogger<ApiResource> logger, MySqlStoreOptions storeOption)
        {
            _logger = logger;
            _storeOptions = storeOption;
        }

        /// <summary>
        /// 根据资源名称获取单个资源
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns></returns>
        public async Task<ApiResource> FindResource(string name)
        {
            var model = new ApiResource();
            using (var connection = new SqlConnection(_storeOptions.ConnectionStrings))
            {
                var query = await connection.QueryMultipleAsync(_sql1, new { name }).ConfigureAwait(false);
                var apiResources = query.Read<IdentityApiResource>();
                var scopes = query.Read<IdentityScope>();
                if(apiResources !=null && apiResources.AsList()?.Count > 0)
                {
                    var resource = apiResources.AsList()[0];
                    resource.Scopes = scopes.ToList();
                }
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApiResource>> FindResourcesByScopes(IEnumerable<string> scopes)
        {
            var apiResourceData = new List<ApiResource>();
            string _scopes = string.Empty;
            foreach (var scope in scopes)
            {
                _scopes += "'" + scope + "',";
            }
            if (_scopes == "")
            {
                return null;
            }
            else
            {
                _scopes = _scopes.Substring(0, _scopes.Length - 1);
            }

            var sql = string.Format(_sql2, scopes);
            using (var connection = new SqlConnection(_storeOptions.ConnectionStrings))
            {
                var apir = (await connection.QueryAsync<IdentityApiResource>(sql))?.AsList();
                if (apir != null && apir.Count > 0)
                {
                    foreach (var apimodel in apir)
                    {
                        sql = _sql3;
                        var scopedata = (await connection.QueryAsync<IdentityScope>(sql, new { id = apimodel.Id }))?.AsList();
                        apimodel.Scopes = scopedata;
                       // apiResourceData.Add(apimodel.ToModel());
                    }
                    _logger.LogDebug("Found {scopes} API scopes in database", apiResourceData.SelectMany(x => x.Scopes).Select(x => x.Name));
                }
            }
            return apiResourceData;
        }

        private readonly MySqlStoreOptions _storeOptions;
        private readonly ILogger<ApiResource> _logger;
        private string _sql1 = @"select * from ApiResources where Name=@Name and Enabled=1;
                       select * from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t1.Name=@name and Enabled=1;";
        private string _sql2 = @"select distinct t1.* from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t2.Name in({0}) and Enabled=1;";
        private string _sql3 = @"select * from ApiScopes where ApiResourceId=@id";

    }

}
