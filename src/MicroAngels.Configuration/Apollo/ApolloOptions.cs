using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MicroAngels.Configuration.Apollo
{

	public class ApolloOptions:IApolloOptions
	{
		public string AppId { get; set; }
		public string MetaServer { get; set; }
		public string[] Namespace { get; set; }

		public string DataCenter { get; set; }
		public string Cluster { get; set; }
		public Env Env { get; set; }
		public string SubEnv { get; set; }
		public string LocalIp { get; set; }
		public IReadOnlyCollection<string> ConfigServer { get; set; }
		public int Timeout { get; set; }
		public string Authorization { get; set; }
		public int RefreshInterval { get; set; }
		public string LocalCacheDir { get; set; }
		public Func<HttpMessageHandler> HttpMessageHandlerFactory => throw new NotImplementedException();
	}

}
