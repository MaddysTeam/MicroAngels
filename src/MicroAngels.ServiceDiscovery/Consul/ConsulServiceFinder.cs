using MicroAngels.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.ServiceDiscovery.Consul
{
    class ConsulServiceFinder : IServiceFinder<ConsulService>
    {
        public Task<IList<ConsulService>> FindAsync(string id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ConsulService>> FindAsync(string id, string name, string version)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ConsulService>> FindAsync(string id, string name, string version, string status)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ConsulService>> FindAsync(Predicate<IService> condition)
        {
            throw new NotImplementedException();
        }
    }
}
