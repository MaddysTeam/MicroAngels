using MicroAngels.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace MicroAngels.GRPC.Client
{

	public static class GRPCClientExtensions
	{

		public static IServiceCollection AddGRPCClient(this IServiceCollection services)
		{
			// TODO: you can di your own balancer or service finder here
			services.AddTransient<ILoadBalancer, WeightRoundBalancer>();
			services.AddTransient<IGRPCServiceFinder, GRPCConsulServiceFinder>();
			services.AddTransient<IGRPCConnection, GRPCConnection>();

			return services;
		}

	}

}
