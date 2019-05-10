using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Gateway.Ocelot
{

	public static class OcelotServiceColleciton
	{

		public static IOcelotBuilder AddAngelsOcelot(this IOcelotBuilder builder, Action<OcelotConfiguration> ocelotService)
		{
			builder.Services.Configure(ocelotService);
			builder.Services.AddSingleton(
			  resolver => resolver.GetRequiredService<IOptions<OcelotConfiguration>>().Value);

			// di for file configuration repository
			builder.Services.AddSingleton<IFileConfigurationRepository, MysqlFileConfigurationRepository>();

			// di for redis cache
			builder.Services.AddSingleton<IOcelotCache<FileConfiguration>,OcelotRedisCache<FileConfiguration>>();
			builder.Services.AddSingleton<IOcelotCache<CachedResponse>,OcelotRedisCache<CachedResponse>>();

			// di for authentication service
			builder.Services.AddSingleton<ICustomAuthenticateService, CustomAuthenticateService>();

			return builder;
		}

	}

}
