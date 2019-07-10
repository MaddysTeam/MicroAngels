using MicroAngels.Core.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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


		public static IServiceCollection AddServiceFinder(this IServiceCollection  services, ConsulHostConfiguration configuration)
		{
			services.AddSingleton(configuration);
			services.AddTransient<IServiceFinder<ConsulService>, ConsulServiceFinder>();

			return services;
		}

    }

}
