using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroAngels.Core.Service
{

    public interface IServiceFinder<Service> where Service:IService
    {
        Task<IList<Service>> FindAsync(string id, string name);
        Task<IList<Service>> FindAsync(string id, string name, string version);
        Task<IList<Service>> FindAsync(string id, string name, string version, string status);
        Task<IList<Service>> FindAsync(Predicate<IService> condition);
    }

}
