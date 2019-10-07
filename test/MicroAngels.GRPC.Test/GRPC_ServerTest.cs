using MicroAngels.Core;
using MicroAngels.GRPC.Server;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MicroAngels.GRPC.Test
{

	public class GRPC_ServerTest
	{

		[Fact]
		public void BuildTest()
		{
			var builder = new GRPCServerBuilder();

			Assert.Throws<AngleExceptions>(() =>
			{
				builder.Build(null);
			});

			Assert.Throws<AngleExceptions>(() =>
			{
				builder.Build(new GRPCServerOptions {  });
			});

			Assert.Throws<AngleExceptions>(() =>
			{
				builder.Build(new GRPCServerOptions { Host="127.0.0.1" });
			});
		}

	}

}
