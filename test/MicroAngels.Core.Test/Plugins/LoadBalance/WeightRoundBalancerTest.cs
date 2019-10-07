using MicroAngels.Core.Plugins;
using MicroAngels.Core.Test.Models;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace MicroAngels.Core.Test.Plugins
{

	public class WeightRoundBalancerTest
	{
		private WeightRoundBalancer _balancer;
		private Dictionary<Services, int> _services;

		public WeightRoundBalancerTest()
		{
			_balancer = new WeightRoundBalancer();
		}

		[Fact]
		public void BalanceTest()
		{

			_services = new Dictionary<Services, int> {
				{ new Services { Name="service1" }, 1 },
				{ new Services { Name="service2" }, 100 },
				{ new Services { Name="service3" }, 20 }
			};

			var serviceList = new List<string>();

			for (int i = 0; i < 1000; i++)
			{
				serviceList.Add(_balancer.Balance(_services).Name);
			}

			var service1Count = serviceList.Count(x => x == "service1");
			var service2Count = serviceList.Count(x => x == "service2");
			var service3Count = serviceList.Count(x => x == "service3");
			
			Assert.True(new int[] { service1Count, service2Count, service3Count }.Max()== service2Count);
			Assert.True(new int[] { service1Count, service2Count, service3Count }.Min() == service1Count);
		}

	}

}
