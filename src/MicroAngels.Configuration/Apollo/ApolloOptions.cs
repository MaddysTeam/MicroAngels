﻿using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MicroAngels.Configuration.Apollo
{

	public class ApolloOptions : IApolloOptions
	{
		public string AppId { get; set; }
		public string MetaServer { get; set; }
		public string[] Namespace { get; set; }
		public string DataCenter { get; set; }
		public string Cluster { get; set; }
		public Env Env { get; private set; }
		public string SubEnv { get; }
		public string LocalIp { get; }
		public IReadOnlyCollection<string> ConfigServer { get; set; }
		public int Timeout { get; set; }
		public string Authorization { get; }
		public int RefreshInterval { get; set; }
		public string LocalCacheDir { get; set; }
		public Func<HttpMessageHandler> HttpMessageHandlerFactory { get; set; }
	}

}
