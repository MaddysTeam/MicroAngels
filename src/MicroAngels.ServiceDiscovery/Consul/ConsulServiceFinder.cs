using Consul;
using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.ServiceDiscovery.Consul
{

	public class ConsulServiceFinder : IServiceFinder<ConsulService>
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

		public Task<IList<ConsulService>> FindAsync(Predicate<IService> condition)
		{
			throw new NotImplementedException();
		}

		public async Task<ConsulService> FindAsync(string id)
		{
			var services = (await _client.Agent.Services()).Response;

			foreach (var service in services.Values)
			{
				if (service.ID == id)
				{
					return new ConsulService
					{
						Id = service.ID,
						Name = service.Service,
						Port = service.Port,
						Host = service.Address,
					};
				}
			}

			return null;

		}

		public async Task<IList<ConsulService>> FindByNameAsync(string name)
		{
			var results = new List<ConsulService>();
			var services = (await _client.Agent.Services()).Response;
			await _client.Agent.ServiceDeregister("f0e1290f-5905-40b0-80b1-d4458cf28bd4");
			foreach (AgentService service in services.Values)
			{
				if (string.Equals(service.Service, name, StringComparison.InvariantCultureIgnoreCase))
				{
					var health = await _client.Health.Checks(service.Service);
					results.Add(new ConsulService
					{
						Id = service.ID,
						Name = service.Service,
						Port = service.Port,
						Host = service.Address,
						Tags = service.Tags,
						HealthStatus = health.Response[0].Status.Status,
					});
				}
			}

			return results;
		}



		private ConsulClient _client;

	}

}
