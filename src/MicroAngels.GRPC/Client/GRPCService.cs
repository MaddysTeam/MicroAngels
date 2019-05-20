using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.GRPC.Client
{

	public class GRPCService
	{
		public string Host { get; }
		public int Port { get; }

		public GRPCService(string host,int port)
		{
			Host = host;
			Port = port;
		}
	}

}
