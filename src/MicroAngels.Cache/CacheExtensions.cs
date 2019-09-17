using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MicroAngels.Cache
{

	public static class CacheExtensions
	{

		public static IServiceContainer AddCacheInterceptorContainer(this IServiceCollection services)
		{
			return services.ToServiceContainer();
		}

		public static IServiceContainer AddInterceptor<T>(this IServiceContainer container,Type cachingType)
		{
			container.AddType<CachingInterceptor<T>>();
			container.Configure(config =>
			{
				config.Interceptors.AddTyped<CachingInterceptor<T>>(method => cachingType.IsAssignableFrom(method.DeclaringType));
			});

			return container;
		}

		public static IServiceProvider BuildProvider(this IServiceContainer container)
		{
			return container.Build();
		}

	}

}
