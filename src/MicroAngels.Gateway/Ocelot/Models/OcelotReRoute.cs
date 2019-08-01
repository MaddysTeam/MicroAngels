namespace MicroAngels.Gateway.Ocelot
{

	internal class OcelotReRoute
	{
		public int ReRouteId { get; set; }
		public int ItemId { get; set; }
		public string UpstreamPathTemplate { get; set; }
		public string UpstreamHttpMethod { get; set; }
		public string UpstreamHost { get; set; }
		public string DownstreamScheme { get; set; }
		public string DownstreamPathTemplate { get; set; }
		public string DownstreamHostAndPorts { get; set; }
		public string AuthenticationOptions { get; set; }
		public string RequestIdKey { get; set; }
		public string CacheOptions { get; set; }
		public string ServiceName { get; set; }
		public string LoadBalancerOptions { get; set; }
		public string QoSOptions { get; set; }
		public string DelegatingHandlers { get; set; }
		public int? Priority { get; set; }
		public int? InfoStatus { get; set; }
	}

}
