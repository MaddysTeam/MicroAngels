using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Core.Service
{

    public interface IServiceChecker
    {
        Task<string> HealthCheck(string serviceId);
    }

}
