using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Core.Service
{

    public interface IServiceRegiter<Service, Error> where Service:IService 
                                                      where Error:IServiceError
    {
        Task<Error> RegistAsync(Service service);
        Task<Error> DeregisterAsync(Service service);
    }

}
