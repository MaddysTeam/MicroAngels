using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Service
{

    public interface IService
    {
        string Id { get; }
        string Name { get; }
        string Version { get; }
        string Address { get; }
        string Port { get; }
        string Group { get; }
        string Doc { get; }
        string HealthStatus { get; }
        DateTime RegistDate { get; }
        DateTime OfflineDate { get; }
    }


    //public interface IRegistedService: IService
    //{

    //}


}
