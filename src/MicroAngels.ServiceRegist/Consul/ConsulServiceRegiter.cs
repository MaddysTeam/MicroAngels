using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MicroAngels.Core.Service;

namespace MicroAngels.ServiceRegistry.Consul
{

    public class ConsulServiceRegiter : IServiceRegiter<ConsulService, ConsulServiceError>
    {

        public Task<ConsulServiceError> DeregisterAsync(ConsulService service)
        {
            throw new NotImplementedException();
        }

        public Task<ConsulServiceError> RegistAsync(ConsulService service)
        {
            throw new NotImplementedException();
        }

    }

}
