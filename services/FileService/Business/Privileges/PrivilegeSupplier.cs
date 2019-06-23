using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Core.Plugins.Auth;
using MicroAngels.Core.Service;
using MicroAngels.ServiceDiscovery.Consul;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileService.Business
{

	public class PrivilegeSupplier : IPrivilegeSupplier
	{

		public PrivilegeSupplier(HttpClient client, IServiceFinder<ConsulService> serviceFinder, ILoadBalancer balancer)
		{
			_client = client;
			_serviceFinder = serviceFinder;
			_balancer = balancer;
		}

		/// <summary>
		/// get urls from remote auth server 
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public async Task<string[]> GetUrls(string[] roles)
		{
			var services = await _serviceFinder.FindByNameAsync(serviceName);
			if (services.IsNull() || services.Count <= 0) return null;

			var service = _balancer.Balance(
				 services.ToDictionary(x => x, y => y.Weight) // loadblance by service weight
				);

			return await _client.PostAsync<string[]>($"http://{service.Host}:{service.Port}/{remoteApi}", roles);
		}

		private readonly HttpClient _client;
		private readonly IServiceFinder<ConsulService> _serviceFinder;
		private readonly ILoadBalancer _balancer;

		//TODO: can put following infomation into applo configuration
		private readonly string serviceName = "AuthService";
		private readonly string remoteApi = "api/assets/getUrls";
	}

}
