using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Gateway.Ocelot
{

    public static class OcelotServiceColleciton
    {

        public static IOcelotBuilder AddAngelsOcelot(IOcelotBuilder builder,Action<OcelotConfiguration> ocelotService)
        {
            builder.Services.Configure(ocelotService);
            builder.Services.AddSingleton(
              resolver => resolver.GetRequiredService<IOptions<OcelotConfiguration>>().Value);
            builder.Services.AddSingleton<IFileConfigurationRepository, MysqlFileConfigurationRepository>();

            return builder;
        }

    }

}
