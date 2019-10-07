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

	public class CustomAuthenticateService : ICustomAuthenticateService
	{

		public CustomAuthenticateService(IServiceFinder<ConsulService> serviceFinder, IConfiguration configuration)
		{
			_serviceFinder = serviceFinder;
			_configuration = configuration;
		}

		/// <summary>
		/// ocelot authorization，interface permissions
		/// if user role is null and return anonymous interface
		/// </summary>
		/// <param name="context">ocelot down stream context</param>
		/// <returns>authorized result</returns>
		public async Task<bool> ValidateAuthenticate(DownstreamContext context)
		{
			var requestPath = context.DownstreamRequest.AbsolutePath;
			var roleClaims = context.HttpContext.User.Claims.Where(c => c.Value ==CoreKeys.ROLE);
			var roles = roleClaims.Count() <= 0 ? new string[] { } : roleClaims.Select(x => x.Type).ToArray();

			//if (context.HttpContext.User.IsNull())
			//{
			//	var permissionUrls = await client.PostAsync<string[]>(serivceUrl, roles);
			//}

			// find interface service
			var services = await _serviceFinder.FindByNameAsync(_configuration["AuthService:Name"]);
			if (!services.IsNull() && services.Count > 0)
			{
				var serivceUrl = $@"{services[0].Address}{_configuration["AuthService:ServiceUrls"]}";
				using (var client = new HttpClient())
				{
					var permissionUrls = await client.PostAsync<string[]>(serivceUrl, roles);

					return !permissionUrls.IsNull() && permissionUrls.Contains(requestPath);
				}
			}

			context.Errors.Add(new UnauthenticatedError("auth service not exit"));

			return false;
		}

		IServiceFinder<ConsulService> _serviceFinder;
		IConfiguration _configuration;

	}

}
