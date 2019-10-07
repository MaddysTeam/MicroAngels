using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Core.Test.Models;
using MicroAngels.GRPC.Client;
using MicroAngels.GRPC.Test.Models;
using Xunit;

namespace MicroAngels.GRPC.Test
{

	public class GRPC_ClientTest
	{

		GRPCConnection _conn;
		private readonly GRPCServiceFinder _grpcServiceFinder = new GRPCServiceFinder();
		private readonly ILoadBalancer _balancer = new WeightRoundBalancer();

		public GRPC_ClientTest()
		{

		}

		[Fact]
		public void GRPCConnectionTest()
		{
			Assert.Throws<AngleExceptions>(() =>
			{
				_conn = new GRPCConnection(null, null);
			});

			_conn = new GRPCConnection(_balancer, _grpcServiceFinder);
			Assert.NotNull(_conn);
		}

		[Fact]
		public async void GetGRPCServiceTest()
		{
			_conn = new GRPCConnection(_balancer, _grpcServiceFinder);
			var grpcService = await _conn.GetGRPCService<IGrpcService>(string.Empty);

			Assert.Null(grpcService);
		}

		[Fact]
		public void ExecuteGRPCServiceTest()
		{

		}

		[Fact]
		public void GrpcConsulServiceFinderTest()
		{

		}

	}

}
