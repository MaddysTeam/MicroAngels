using Grpc.Core;
using MagicOnion.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.GRPC.Server
{

	public class GRPCServerBuilder
	{

		public GRPCServerBuilder BuildServer(GRPCServerOptions options)
		{
			if (options == null) throw new ArgumentNullException("GRPC Server Options can't be null");

			MagicOnionServiceDefinition service = MagicOnionEngine.BuildServerServiceDefinition(new MagicOnionOptions
			{
				MagicOnionLogger = new MagicOnionLogToGrpcLogger()
			});

			var server =new Grpc.Core.Server
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
