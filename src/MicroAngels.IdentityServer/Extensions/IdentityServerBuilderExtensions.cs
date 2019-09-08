using IdentityServer4.Stores;
using MicroAngels.IdentityServer.Providers;
using MicroAngels.IdentityServer.Providers.MySql;
using MicroAngels.IdentityServer.Providers.Redis;
using MicroAngels.IdentityServer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.IdentityServer.Extensions
{

	public static class IdentityServerBuilderExtensions
	{

		public static IIdentityServerBuilder UseMysql(this IIdentityServerBuilder builder, Action<MySqlStoreOptions> storeOptionAction)
		{
			var storeOption = new MySqlStoreOptions();
			storeOptionAction?.Invoke(storeOption);

			builder.Services.AddSingleton<MySqlStoreOptions>(storeOption);
			builder.Services.AddTransient<IClientProvider, MySqlClientProvider>();
			builder.Services.AddTransient<IResourceProvider, MySqlResourceProvider>();
			builder.Services.AddTransient<IGrantStoreProvider, MySqlGrantStoreProvider>();
			builder.Services.AddTransient<IClientStore, ClientStore>();
			builder.Services.AddTransient<IResourceStore, ResourceStore>();
			builder.Services.AddTransient<IPersistedGrantStore, GrantStore>();

			return builder;
		}

		public static IIdentityServerBuilder UseValidateService<T>(this IIdentityServerBuilder builder) where T : class, IUserValidateService
		{
			builder.Services.AddTransient<IUserValidateService, T>();

			return builder;
		}

		public static IIdentityServerBuilder UseClaimsGrantService<T>(this IIdentityServerBuilder builder) where T : class, IClaimsGrantService
		{
			builder.Services.AddTransient<IClaimsGrantService, T>();

			return builder;
		}


		public static IIdentityServerBuilder UseRedisGrantStore(this IIdentityServerBuilder builder, RedisOperationalStoreOptions storeOptionAction)
		{
			//var storeOption = new RedisStoreOptions();
			//storeOptionAction?.Invoke(storeOption);
			builder.Services.AddSingleton(storeOptionAction);

			builder.Services.AddTransient<IGrantStoreProvider, RedisGrantStoreProvider>();
			//builder.Services.AddTransient<IPersistedGrantStore, GrantStore>();

			return builder;
		}

	}

}
