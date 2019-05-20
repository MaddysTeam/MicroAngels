﻿using Grpc.Core;
using MagicOnion;
using MagicOnion.Client;
using MicroAngels.Core.Plugins;
using System.Threading.Tasks;

namespace MicroAngels.GRPC.Client
{

	public interface IGRPCConnection
	{
		Task<TService> GetGRPCService<TService>(string serviceName) where TService : IService<TService>;
		Task<TService> GetGRPCService<TService>(GRPCService service) where TService : IService<TService>;
	}

	public class GRPCConnection : IGRPCConnection
	{

		public GRPCConnection(ILoadBalancer balancer, IGRPCServiceFinder serviceFinder)
		{
			_balancer = balancer ?? new WeightRoundBalancer();

			if (serviceFinder != null)
				_serviceFinder = serviceFinder;
		}

		public async Task<TService> GetGRPCService<TService>(string serviceName) where TService : IService<TService>
		{
			var services = await _serviceFinder.GetServicesAsync(serviceName);
			var service = _balancer.Balance(services);

			var channel = new Channel(service.Host, service.Port, ChannelCredentials.Insecure);
			var result = MagicOnionClient.Create<TService>(channel);

			return result;
		}

		public async Task<TService> GetGRPCService<TService>(GRPCService service) where TService : IService<TService>
		{
			if (service == null) return default(TService);

			var channel = new Channel(service.Host, service.Port, ChannelCredentials.Insecure);
			return MagicOnionClient.Create<TService>(channel);
		}

		private ILoadBalancer _balancer;
		private IGRPCServiceFinder _serviceFinder;

	}

}