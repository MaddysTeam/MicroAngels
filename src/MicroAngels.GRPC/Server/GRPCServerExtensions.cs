using MicroAngels.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.GRPC.Server
{

	public static class GRPCServerExtensions
	{

		public static GRPCServerBuilder AddGRPCServer(this IServiceCollection collection,GRPCServerOptions options)
		{
			return new GRPCServerBuilder().BuildServer(options);
		}

		public static GRPCServerBuilder AddGRPCServer(this IServiceCollection collection, Action<GRPCServerOptions> options)
		{
			//return new GRPCServerBuilder().BuildServer(options);
			throw new NotSupportedException();
		}

	}

}
