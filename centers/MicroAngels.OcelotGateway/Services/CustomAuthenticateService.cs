using MicroAngels.Core.Service;
using MicroAngels.Gateway.Ocelot;
using MicroAngels.ServiceDiscovery.Consul;
using MicroAngels.Core;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class CustomAuthenticateService : ICustomAuthenticateService
	{

		public CustomAuthenticateService(IServiceFinder<ConsulService> serviceFinder, IConfiguration configuration)
		{
			_serviceFinder = serviceFinder;
			_configuration = configuration;
		}

		/// <summary>
		/// ocelot authorization，interface permissions
		/// </summary>
		/// <param name="context">ocelot down stream context</param>
		/// <returns>authorized result</returns>
		public async Task<bool> ValidateAuthenticate(DownstreamContext context)
		{
			var requestPath = context.DownstreamRequest.AbsolutePath;
			var roleClaims = context.HttpContext.User.Claims.Where(c => c.Value == "role");
			if (roleClaims.Count() > 0)
			{
				var roles = roleClaims.Select(x => x.Type).ToArray();
				var rolesString = string.Join(",", roles);
				// call remote auth service to get urls
				var services = await _serviceFinder.FindByNameAsync(_configuration["RemoteService:Name"]);
				if (!services.IsNull() && services.Count > 0)
				{
					var serivceUrl = $@"{services[0].Address}{_configuration["RemoteService:Url"]}?roles={rolesString}";
					using (var client = new HttpClient())
					{
						var permissionUrls = await client.GetAsync<string[]>(serivceUrl);

						return !permissionUrls.IsNull() && permissionUrls.Contains(requestPath);
					}
				}

				return false;
			}

			return false;
		}

		IServiceFinder<ConsulService> _serviceFinder;
		IConfiguration _configuration;

	}

}
