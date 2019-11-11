using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using System;

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
			// di for cache response
			builder.Services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCache<CachedResponse>>();

			return builder;
		}

	}


	#region temp

	// di for redis cache
	//builder.Services.AddSingleton<IOcelotCache<FileConfiguration>,OcelotRedisCache<FileConfiguration>>();
	//builder.Services.AddSingleton<IOcelotCache<CachedResponse>,OcelotRedisCache<CachedResponse>>();

	//builder.Services.AddSingleton<IOcelotCache<RefreshToken>, OcelotRedisCache<RefreshToken>>();

	// di for authentication service
	//builder.Services.AddSingleton<ICustomAuthenticateService, DefaultCustomAuthenticateService>();

	#endregion

}
