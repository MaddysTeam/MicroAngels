using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Gateway.Ocelot
{

    public class OcelotConfiguration 
    {

		public string ConnectString { get; set; }

		public string[] RedisConnectStrings { get; set; }

		public bool IsUseCustomAuthenticate { get; set; }

		public string CacheKeyPrefix { get; set; }

		public TimeSpan TokenRefreshIterval { get; set; }

    }

}
