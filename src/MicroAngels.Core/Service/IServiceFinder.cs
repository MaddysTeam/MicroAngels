using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroAngels.Core.Service
{

    public interface IServiceFinder<Service> where Service:IService
    {
        Task<Service> FindAsync(string id);
		Task<IList<Service>> FindByNameAsync(string name);
		Task<IList<Service>> FindAsync(Predicate<IService> condition);
    }

}
