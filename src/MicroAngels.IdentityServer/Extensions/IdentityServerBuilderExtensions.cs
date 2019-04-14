using IdentityServer4.Configuration;
using IdentityServer4.Stores;
using MicroAngels.IdentityServer.Providers;
using MicroAngels.IdentityServer.Providers.MySql;
using MicroAngels.IdentityServer.Providers.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace MicroAngels.IdentityServer.Extensions
{

    public static class IdentityServerBuilderExtensions
    {

        public static IIdentityServerBuilder UseMysql(this IIdentityServerBuilder builder, bool useMysqlGrantStore = false)
        {
            builder.Services.AddTransient<IClientProvider, MySqlClientProvider>();
            builder.Services.AddTransient<IResourceProvider, MySqlResourceProvider>();
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            
            return builder;
        }

        public static IIdentityServerBuilder UseRedisGrantStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IGrantStoreProvider, RedisGrantStoreProvider>();
            builder.Services.AddTransient<IPersistedGrantStore, GrantStore>();

            return builder;
        }


    }

}
