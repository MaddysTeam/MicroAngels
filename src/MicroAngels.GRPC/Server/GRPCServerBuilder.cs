using Grpc.Core;
using MagicOnion.Server;
using MicroAngels.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.GRPC.Server
{

	public class GRPCServerBuilder
	{

		public GRPCServerBuilder Build(GRPCServerOptions options)
		{
			if (options.IsNull()) throw new MicroAngels.Core.AngleExceptions("GRPC Server Options can't be null");
			if (options.Host.IsNullOrEmpty()) throw new MicroAngels.Core.AngleExceptions("GRPC Server host is required");
			if (options.Port <= 0) throw new MicroAngels.Core.AngleExceptions("GRPC Server port is required");

			MagicOnionServiceDefinition service = MagicOnionEngine.BuildServerServiceDefinition(new MagicOnionOptions
			{
				MagicOnionLogger = new MagicOnionLogToGrpcLogger()
			});

			var server = new Grpc.Core.Server
			{
				Services = { service },
				Ports = { new ServerPort(
					 options.Host,
					 options.Port,
					 ServerCredentials.Insecure
					) }
			};

			server.Start();

			return this;
		}



	}

}
