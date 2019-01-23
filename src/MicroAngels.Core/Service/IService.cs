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
        Uri Address { get; }
        string Host { get; }
        int Port { get; }
        string Group { get; }
        string Doc { get; }
        string HealthStatus { get; }
    }

}
