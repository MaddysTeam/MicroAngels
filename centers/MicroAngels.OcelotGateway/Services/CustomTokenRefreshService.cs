﻿using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using MicroAngels.Core;
using MicroAngels.Gateway.Ocelot;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using MicroAngels.Cache.Redis;

namespace MicroAngels.OcelotGateway.Services
{

	public class CustomTokenRefreshService : ICustomTokenRefreshService
	{

		public CustomTokenRefreshService(IServiceFinder<ConsulService> serviceFinder, ILoadBalancer loadBalancer, IRedisCache cache, OcelotConfiguration options, IConfiguration configuration)
		{
			_serviceFinder = serviceFinder;
			_loadBalancer = loadBalancer ?? new WeightRoundBalancer();
			_options = options;
			_cache = cache;
		}

		public async Task<DownstreamContext> Refresh(DownstreamContext context)
		{
			// 1 get userid
			// 2 get refresh date from redis by using user id
			// 3 refresh token when token response cache is null

			var userId = context.HttpContext.User.GetUserId();
			if (userId.IsNull() || context.DownstreamRequest.AbsolutePath.IndexOf("briefInfo") < 0)
				return context;

			if(_cache.Lock(userId, _options.TokenRefreshIterval))
			{
				var rk = context.HttpContext.Request.Headers[CoreKeys.RefreshToken].ToString();
				// refresh token
				if (context.DownstreamReRoute.IsAuthenticated && !rk.IsNullOrEmpty() && rk != "null")
				{
					using (var client = new HttpClient())
					{
						var tokenResponse = await client.PostAsync<TokenResponse, ConsulService>("AccountService", "api/account/refresh", null, new { RefreshToken = rk }, _serviceFinder, _loadBalancer);
						if (tokenResponse.IsValidate)
						{
							tokenResponse.LastUpdateDate = DateTime.UtcNow;
							RefreshTokenInHeaders(context,tokenResponse);
						}
					}
				}
			}
			else
			{
				context.DownstreamRequest.Headers.Remove(CoreKeys.RefreshToken);
				context.DownstreamRequest.Headers.Remove(CoreKeys.AccessToken);
			}

			return context;
		}


		private void RefreshTokenInHeaders(DownstreamContext context, TokenResponse tokenResponse)
		{
			context.DownstreamRequest.Headers.Remove(CoreKeys.RefreshToken);
			context.DownstreamRequest.Headers.Remove(CoreKeys.AccessToken);
			context.DownstreamRequest.Headers.Add(CoreKeys.RefreshToken, tokenResponse.RefreshToken);
			context.DownstreamRequest.Headers.Add(CoreKeys.AccessToken, tokenResponse.Token);
		}

		private IServiceFinder<ConsulService> _serviceFinder;
		private ILoadBalancer _loadBalancer;
		private IRedisCache _cache;
		private OcelotConfiguration _options;

	}

	public class TokenResponse 
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public DateTime LastUpdateDate { get; set; }

		public bool IsValidate => !Token.IsNullOrEmpty() && !RefreshToken.IsNullOrEmpty();
	}


}
