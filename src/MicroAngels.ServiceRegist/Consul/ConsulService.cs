using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.ServiceRegistry.Consul
{

    public class ConsulService : IService
    {
        public string Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Version => throw new NotImplementedException();

        public string Address => throw new NotImplementedException();

        public string Port => throw new NotImplementedException();

        public string Group => throw new NotImplementedException();

        public string Doc => throw new NotImplementedException();

        public string HealthStatus => throw new NotImplementedException();

        public DateTime RegistDate => throw new NotImplementedException();

        public DateTime OfflineDate => throw new NotImplementedException();
    }


    public class ConsulServiceError : IServiceError
    {
        public string ServiceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Id => throw new NotImplementedException();

        public string Message => throw new NotImplementedException();

        public string Level => throw new NotImplementedException();

        public IList<Exception> Inner => throw new NotImplementedException();
    }

}
