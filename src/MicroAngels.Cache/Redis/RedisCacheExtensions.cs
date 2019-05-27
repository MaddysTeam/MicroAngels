using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MicroAngels.Cache.Redis
{

	public static class RedisCacheExtensions
	{

		public static IServiceCollection AddRedisCache(this IServiceCollection builder, RedisCacheOption redisOptions)
		{
			builder.AddSingleton(redisOptions);
			builder.AddTransient<IRedisCache, RedisCache>();

			return builder;
		}

		public static IServiceCollection AddRedisCache(this IServiceCollection builder, IEnumerable<RedisCacheOption> redisOptions)
		{
			builder.AddSingleton(redisOptions);
			builder.AddTransient<IRedisCache, RedisCache>();

			return builder;
		}

	}

}
