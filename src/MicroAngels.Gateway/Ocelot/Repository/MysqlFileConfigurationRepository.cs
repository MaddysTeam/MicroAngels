using Dapper;
using MicroAngels.Core;
using MySql.Data.MySqlClient;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	/// <summary>
	/// get ocelot configuration data from my sql
	/// </summary>
	public class MysqlFileConfigurationRepository : IFileConfigurationRepository
	{
		//get configuration from db

		public MysqlFileConfigurationRepository(OcelotConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<Response<FileConfiguration>> Get()
		{
			var file = new FileConfiguration();

			using (var connection = new MySqlConnection(_configuration.ConnectString))
			{
				var result = await connection.QueryFirstOrDefaultAsync<OcelotGlobalConfiguration>(_gloableSql);
				if (result != null)
				{
					// get global configuration
					var glb = new FileGlobalConfiguration();
					glb.BaseUrl = result.BaseUrl;
					glb.DownstreamScheme = result.DownstreamScheme;
					glb.RequestIdKey = result.RequestIdKey;
					if (!String.IsNullOrEmpty(result.HttpHandlerOptions))
					{
						glb.HttpHandlerOptions = result.HttpHandlerOptions.ToObject<FileHttpHandlerOptions>();
					}
					if (!String.IsNullOrEmpty(result.LoadBalancerOptions))
					{
						glb.LoadBalancerOptions = result.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
					}
					if (!String.IsNullOrEmpty(result.QoSOptions))
					{
						glb.QoSOptions = result.QoSOptions.ToObject<FileQoSOptions>();
					}
					if (!String.IsNullOrEmpty(result.ServiceDiscoveryProvider))
					{
						glb.ServiceDiscoveryProvider = result.ServiceDiscoveryProvider.ToObject<FileServiceDiscoveryProvider>();
					}
					file.GlobalConfiguration = glb;

					//get re route info
					var routeresult = (await connection.QueryAsync<OcelotReRoute>(_routeSql, new { result.OcelotId }))?.AsList();
					if (routeresult != null && routeresult.Count > 0)
					{
						var reroutelist = new List<FileReRoute>();
						foreach (var model in routeresult)
						{
							var m = new FileReRoute();

							if (!String.IsNullOrEmpty(model.AuthenticationOptions))
							{
								m.AuthenticationOptions = model.AuthenticationOptions.ToObject<FileAuthenticationOptions>();
							}
							if (!String.IsNullOrEmpty(model.CacheOptions))
							{
								m.FileCacheOptions = model.CacheOptions.ToObject<FileCacheOptions>();
							}
							if (!String.IsNullOrEmpty(model.DelegatingHandlers))
							{
								m.DelegatingHandlers = model.DelegatingHandlers.ToObject<List<string>>();
							}
							if (!String.IsNullOrEmpty(model.LoadBalancerOptions))
							{
								m.LoadBalancerOptions = model.LoadBalancerOptions.ToObject<FileLoadBalancerOptions>();
							}
							if (!String.IsNullOrEmpty(model.QoSOptions))
							{
								m.QoSOptions = model.QoSOptions.ToObject<FileQoSOptions>();
							}
							if (!String.IsNullOrEmpty(model.DownstreamHostAndPorts))
							{
								m.DownstreamHostAndPorts = model.DownstreamHostAndPorts.ToObject<List<FileHostAndPort>>();
							}

							m.DownstreamPathTemplate = model.DownstreamPathTemplate;
							m.DownstreamScheme = model.DownstreamScheme;
							m.Key = model.RequestIdKey;
							m.Priority = model.Priority ?? 0;
							m.RequestIdKey = model.RequestIdKey;
							m.ServiceName = model.ServiceName;
							m.UpstreamHost = model.UpstreamHost;
							m.UpstreamHttpMethod = model.UpstreamHttpMethod?.ToObject<List<string>>();
							m.UpstreamPathTemplate = model.UpstreamPathTemplate;
							reroutelist.Add(m);
						}
						file.ReRoutes = reroutelist;
					}
				}
				else
				{
					throw new Exception("未监测到任何可用的配置信息");
				}
			}

			if (file.ReRoutes == null || file.ReRoutes.Count == 0)
			{
				return new OkResponse<FileConfiguration>(null);
			}

			return new OkResponse<FileConfiguration>(file);

		}

		public  Task<Response> Set(FileConfiguration fileConfiguration)
		{
			return Task.FromResult(new OkResponse() as Response);
		}

		private OcelotConfiguration _configuration;

		private static string _gloableSql = "select * from OcelotGlobalConfiguration where isDefault=1 and InfoStatus=1";
		private static string _routeSql = "select T2.* from OcelotConfigReRoutes T1 inner join OcelotReRoute T2 on T1.ReRouteId=T2.ReRouteId where OcelotId=@OcelotId and InfoStatus=1";
	}

}
