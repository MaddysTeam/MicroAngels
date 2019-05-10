using Consul;
using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.ServiceDiscovery.Consul
{

	class ConsulServiceFinder : IServiceFinder<ConsulService>
	{

		public ConsulServiceFinder(ConsulHostConfiguration configuration)
		{
			_client = new ConsulClient(config =>
			{
				config.Address = configuration.Address;
				config.Datacenter = configuration.DataCenter;
				config.Token = configuration.Token;
				config.WaitTime = configuration.WaitTime;
			});
		}

		public async Task<IList<ConsulService>> FindAsync(string id, string name)
		{
			var results = new List<ConsulService>();
			var services = (await _client.Agent.Services()).Response;

			foreach (AgentService service in services.Values)
			{
				if (service.ID == id || string.Equals(service.Service, name, StringComparison.InvariantCultureIgnoreCase))
				{
					results.Add(new ConsulService
					{
						Id = service.ID,
						Address = new Uri(service.Address),
						Name=service.Service,
						Port=service.Port,
						Host=service.Address,
					});
				}
			}

			return results;
		}

		public Task<IList<ConsulService>> FindAsync(Predicate<IService> condition)
		{
			throw new NotImplementedException();
		}


		private ConsulClient _client;

	}

}
