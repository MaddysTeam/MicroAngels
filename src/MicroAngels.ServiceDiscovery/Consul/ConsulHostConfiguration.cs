using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.ServiceDiscovery.Consul
{

    public class ConsulHostConfiguration
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string DataCenter { get; set; }
        public string Token { get; set; }
        public TimeSpan? RetryTime { get; set; }
        public TimeSpan? WaitTime { get; set; }

        public Uri Address => new Uri($"{Host}:{Port}");

    }

}
