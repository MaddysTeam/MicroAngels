using Dapper;
using IdentityServer4.Models;
using MicroAngels.Core;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ids4 = IdentityServer4.Models;

namespace MicroAngels.IdentityServer.Providers.MySql
{

	public class MySqlResourceProvider : IResourceProvider
	{

		public MySqlResourceProvider(ILogger logger, MySqlStoreOptions storeOption)
		{
			_logger = logger;
			_storeOptions = storeOption;
		}

		/// <summary>
		/// find resource by name
		/// </summary>
		/// <param name="name">资源名称</param>
		/// <returns></returns>
		public async Task<ApiResource> FindResource(string name)
		{
			if (name.IsNullOrEmpty()) return null;

			var model = new ApiResource();
			using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
			{
				var query = await connection.QueryMultipleAsync(Sqls.ApiResource, new { name }).ConfigureAwait(false);
				var apiResources = query.Read<IdentityApiResource>();
				var scopes = query.Read<IdentityApiScope>();
				var secrets = query.Read<IdentityApiSecret>();
				if (apiResources != null && apiResources.AsList()?.Count > 0)
				{
					var resource = apiResources.AsList()[0];
					resource.Scopes = scopes.ToList();
					resource.Secrets = secrets.ToList();
					model = resource.Map();
				}
			}

			return model;
		}

		/// <summary>
		/// find resources by scopes
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

			var sql = string.Format(Sqls.ApiResourceWithScopes, _scopes);
			using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
			{
				var apir = (await connection.QueryAsync<IdentityApiResource>(sql))?.AsList();
				if (apir != null && apir.Count > 0)
				{
					foreach (var apimodel in apir)
					{
						sql = Sqls.ApiResourceWithScopesByResourceId;
						var scopedata = (await connection.QueryAsync<IdentityApiScope>(sql, new { id = apimodel.Id }))?.AsList();
						apimodel.Scopes = scopedata;
						apiResourceData.Add(apimodel.Map());
					}
					_logger?.Info($"Found { apiResourceData.SelectMany(x => x.Scopes).Select(x => x.Name)} API scopes in database");
				}
			}
			return apiResourceData;
		}


		public async Task<IEnumerable<ids4.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			var apiResourceData = new List<ids4.IdentityResource>();
			string _scopes = "";
			foreach (var scope in scopeNames)
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
			using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
			{
				//暂不实现 IdentityClaims
				string sql = "select * from IdentityResources where Enabled=1 and Name in(" + _scopes + ")";
				var data = (await connection.QueryAsync<MicroAngels.IdentityServer.Models.IdentityResource>(sql))?.AsList();
				if (data != null && data.Count > 0)
				{
					foreach (var model in data)
					{
						apiResourceData.Add(model.Map());
					}
				}
			}
			return apiResourceData;
		}



		/// <summary>
		/// get all resources
		/// </summary>
		/// <returns></returns>
		public async Task<Resources> GetAllResourcesAsync()
		{
			var apiResourceData = new List<ApiResource>();
			var identityResourceData = new List<ids4.IdentityResource>();
			using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
			{
				var sql = Sqls.EvanledIdentityResources;
				var data = (await connection.QueryAsync<Models.IdentityResource>(sql))?.AsList();
				if (data != null && data.Count > 0)
				{
					foreach (var item in data)
					{
						identityResourceData.Add(item.Map());
					}
				}
				//获取apiresource
				sql = Sqls.EnabledApiResources;
				var apidata = (await connection.QueryAsync<IdentityApiResource>(sql))?.AsList();
				if (apidata != null && apidata.Count > 0)
				{
					foreach (var item in apidata)
					{
						sql = Sqls.ApiResourceWithScopesByResourceId;
						var query = await connection.QueryMultipleAsync(sql, new { id = item.Id }).ConfigureAwait(false); //(await connection.QueryAsync<IdentityApiScope>(sql, new { id = item.Id }))?.AsList();
						var scopedata = query.Read<IdentityApiScope>().ToList();
						var secrets = query.Read<IdentityApiSecret>().ToList();
						item.Scopes = scopedata;
						item.Secrets = secrets;
						apiResourceData.Add(item.Map());
					}
				}
			}
			var model = new Resources(identityResourceData, apiResourceData);
			return model;
		}

		private readonly MySqlStoreOptions _storeOptions;
		private readonly ILogger _logger;

	}

}
