using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Cache
{

	public static class CacheExtensions
	{

		public static IServiceContainer AddCacheInterceptorContainer(this IServiceCollection services)
		{
			return services.ToServiceContainer();
		}

		public static IServiceCollection AddInterceptor<T>(this IServiceCollection services,IServiceContainer container,Type cachingType)
		{
			container.AddType<CachingInterceptor<T>>();
			container.Configure(config =>
			{
				config.Interceptors.AddTyped<CachingInterceptor<T>>(method => cachingType.IsAssignableFrom(method.DeclaringType));
			});

			return services;
		}

		public static IServiceProvider Build(this IServiceCollection services, IServiceContainer container)
		{
			return container.Build();
		}

	}

}
