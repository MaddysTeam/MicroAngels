using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;

namespace MicroAngels.ServiceDiscovery.Consul
{

    public class ConsulService : IService
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; private set; }

        public Uri Address { get; private set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Group { get; private set; }

        public string Doc { get; private set; }

        public string HealthStatus { get; private set; }

        //public DateTime RegistDate { get; }

        //public DateTime OfflineDate { get; }

        public string[] Tags { get; }

        public ConsuleHealthCheckOptoins HealthCheckOptoins { get; set; }

        public ConsulHostConfiguration HostConfiguration { get; set; }

    }

    public class ConsuleHealthCheckOptoins : ServiceHealthCheckOptions
    {

    }


    public class ConsulServiceResult : IServiceError
    {
        public bool IsSuccess { get; private set; }

        public string ServiceId { get; private set; }

        public string Message { get; private set; }

        public string Level { get; private set; }

        public IList<Exception> Inner { get; private set; }

        public string Id { get; private set; }

        //public ConsulServiceResult(bool isSuccess,string serviceId, string message, string level)
        //    : this(isSuccess,serviceId, message, level, null) { }

        public ConsulServiceResult(bool isSuccess, string serviceId = null, string message = null, string level = null, IList<Exception> inner = null)
        {
            IsSuccess = isSuccess;
            ServiceId = serviceId;
            Message = message;
            Level = level;
        }

    }

}
