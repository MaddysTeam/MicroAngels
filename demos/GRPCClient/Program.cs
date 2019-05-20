using GRPCService.Models;
using GRPCService.Services;
using MicroAngels.GRPC.Client;
using System;

namespace GRPCClient
{
	class Program
	{
		static void Main(string[] args)
		{
			IGRPCConnection conn = new GRPCConnection(null, null);
			var grpcService = new MicroAngels.GRPC.Client.GRPCService("192.168.1.8", 1999);
			var service = conn.GetGRPCService<IPingService>(grpcService).Result;
			var pong = service.Ping(new Ping { Message = "Hello GRPC" });

			Console.WriteLine("Client Received:" + pong.ResponseAsync.Result.Message);

			Console.ReadLine();
		}

	}
}
