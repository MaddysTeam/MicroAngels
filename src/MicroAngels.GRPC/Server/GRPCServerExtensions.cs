using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.GRPC.Server
{

	public static class GRPCServerExtensions
	{

		public static GRPCServerBuilder AddGRPCServer(this IServiceCollection collection,GRPCServerOptions options)
		{
			return new GRPCServerBuilder().Build(options);
		}

		public static GRPCServerBuilder AddGRPCServer(this IServiceCollection collection, Action<GRPCServerOptions> options)
		{
			//return new GRPCServerBuilder().BuildServer(options);
			throw new NotSupportedException();
		}

	}

}
