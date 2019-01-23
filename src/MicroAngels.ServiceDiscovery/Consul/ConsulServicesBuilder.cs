using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.ServiceDiscovery.Consul
{

    public static class ConsulServicesBuilder
    {

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ConsulService service)
        {
            var serviceRegister = new ConsulServiceRegister(service.HostConfiguration);
            serviceRegister.RegistAsync(service).ConfigureAwait(false).GetAwaiter().GetResult();

            lifetime.ApplicationStopping.Register(() =>
            {
                serviceRegister.DeregisterAsync(service).ConfigureAwait(false).GetAwaiter().GetResult();
            });

            return app;
        }

    }

}
