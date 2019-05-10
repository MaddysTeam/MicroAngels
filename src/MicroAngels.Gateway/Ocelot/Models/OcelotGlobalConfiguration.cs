using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Gateway.Ocelot
{

	internal class OcelotGlobalConfiguration
	{
		public int OcelotId { get; set; }
		public string GatewayName { get; set; }
		public string RequestIdKey { get; set; }
		public string BaseUrl { get; set; }
		public string DownstreamScheme { get; set; }
		public string ServiceDiscoveryProvider { get; set; }
		public string LoadBalancerOptions { get; set; }
		public string HttpHandlerOptions { get; set; }
		public string QoSOptions { get; set; }
		public int IsDefault { get; set; }
		public int InfoStatus { get; set; }
	}

}
