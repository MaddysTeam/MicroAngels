using MicroAngels.Core.Service;
using MicroAngels.ServiceDiscovery.Consul;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroAngels.GRPC.Client
{

	public interface IGRPCServiceFinder
	{
		Task<Dictionary<GRPCService, int>> GetServicesAsync(string serviceName);
	}

	public class GRPCConsulServiceFinder : IGRPCServiceFinder
	{

		public GRPCConsulServiceFinder(IServiceFinder<ConsulService> serviceFinder)
		{
			_serviceFinder = serviceFinder;
		}

		public async Task<Dictionary<GRPCService, int>> GetServicesAsync(string serviceName)
		{
			var results = new Dictionary<GRPCService, int>();
			var services= await _serviceFinder.FindByNameAsync(serviceName);
			foreach(var item in services)
			{
				GRPCService grpc_service = new GRPCService(item.Host,item.Port);
				results.Add(grpc_service, item.Weight);
			}

			return results;
		}

		IServiceFinder<ConsulService> _serviceFinder;

	}

}
