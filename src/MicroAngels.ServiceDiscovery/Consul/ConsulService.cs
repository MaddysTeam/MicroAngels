using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.ServiceDiscovery.Consul
{

    public class ConsulService : IService
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Version { get; private set; }

        public Uri Address { get; private set; }

        public string Host { get; set; }

        public int Port { get; private set; }

        public string Group { get; private set; }

        public string Doc { get; private set; }

        public string HealthStatus { get; private set; }

        public DateTime RegistDate { get; }

        public DateTime OfflineDate { get; }

        public string[] Tags { get; }

        public ConsuleHealthCheckOptoins HealthCheckOptoins { get; set; }

        public ConsulHostConfiguration HostConfiguration { get; set; }

    }

    public class ConsuleHealthCheckOptoins: ServiceHealthCheckOptions
    {

    }


    public class ConsulServiceError : IServiceError
    {

        public string ServiceId { get; private set; }

        public string Message { get; private set; }

        public string Level { get; private set; }

        public IList<Exception> Inner { get; private set; }

        public string Id { get; private set; }

        public ConsulServiceError(string serviceId, string message, string level)
            : this(serviceId, message, level, null) { }

        public ConsulServiceError(string serviceId, string message, string level, IList<Exception> inner)
        {
            ServiceId = serviceId;
            Message = message;
            Level = level;
        }

    }

}
