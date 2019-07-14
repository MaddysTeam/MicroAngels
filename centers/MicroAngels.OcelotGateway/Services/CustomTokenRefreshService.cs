using MicroAngels.Core.Service;
using MicroAngels.Gateway.Ocelot;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Core;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroAngels.OcelotGateway.Services
{

	public class CustomTokenRefreshService : ICustomTokenRefreshService
	{

		public CustomTokenRefreshService(IServiceFinder<ConsulService> serviceFinder, IConfiguration configuration)
		{
		
		}

		public Task<DownstreamContext> Refresh(DownstreamContext context)
		{
			// 1 get userid
			// 2 get refresh date from redis by using userid
			// 3 if Date now minus refresh date is bigger than optinos.TokenRefresh then refresh token

			//var claim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId");
			//if (claim == null)
			//{
			//	await _next.Invoke(context);
			//}

			//var userId = claim.Type;
			//var refreshToken = _cache.Get(userId, null);
			//if (refreshToken.IsNull() || DateTime.Now - refreshToken.LastUpdateDate >= _options.TokenRefreshIterval)
			//{
			//	// update token
			//	var ak = context.HttpContext.Request.Headers[""];
			//	var rk = context.HttpContext.Request.Headers[""];

			//	refreshToken.LastUpdateDate = DateTime.Now;
			//	_cache.Add(userId, refreshToken, TimeSpan.FromDays(30), null);
			//}

			return Task.FromResult(context);
		}

	}

	//public class RefreshToken
	//{
	//	public string Token { get; set; }
	//	public DateTime LastUpdateDate { get; set; }
	//}


}
