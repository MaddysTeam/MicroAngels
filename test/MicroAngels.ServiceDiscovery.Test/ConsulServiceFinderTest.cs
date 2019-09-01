using MicroAngels.ServiceDiscovery.Consul;
using Xunit;
using System.Linq;
using System;

namespace MicroAngels.ServiceDiscovery.Test
{

	public class ConsulServiceFinderTest
	{

		private readonly ConsulServiceFinder _finder;
		private string _serviceName= "AuthService";

		public ConsulServiceFinderTest()
		{
			_finder = new ConsulServiceFinder(
				new ConsulHostConfiguration
				{
					Host = "192.168.1.9",
					Port = 8500
				});
		}


		[Fact]
		public async void FindSingleServiceWithGoodStatusTest()
		{
			var result = await _finder.FindByNameAsync(_serviceName);
			var service = result.First();
			var healths = service.HealthStatus;

			Assert.NotNull(result);
			Assert.NotNull(service);
			Assert.Equal("passing", healths);
			Assert.Equal(1, result.Count);
			Assert.Equal(new Uri("http://192.168.1.2:6000"), service.Address);
		}

		[Fact]
		public async void FindMuliteServiceTest()
		{
			var result = await _finder.FindByNameAsync(_serviceName);
			Assert.True(result.Count > 1);
		}

	}

}
