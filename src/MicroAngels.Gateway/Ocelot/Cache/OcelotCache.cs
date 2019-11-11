using MicroAngels.Cache;
using MicroAngels.Core;
using Ocelot.Cache;
using System;

namespace MicroAngels.Gateway.Ocelot
{

	public class OcelotCache<T> : IOcelotCache<T>
	{

		public OcelotCache(OcelotConfiguration options,ICache redisCache)
		{
			_options = options;
			_cache = redisCache;
			//CSRedisClient redisClient = null;

			//if (options.RedisConnectStrings.Length == 1)
			//{
			//	redisClient = new CSRedisClient(options.RedisConnectStrings[0]);
			//}
			//else
			//{
			//	Func<string, string> keyFunc = null;
			//	redisClient = new CSRedisClient(keyFunc, options.RedisConnectStrings);
			//}

			//RedisHelper.Initialization(redisClient);
		}

		public void Add(string key, T value, TimeSpan ttl, string region)
		{
			var cacheKey = GetCacheKey(region, key);
			if (ttl.Milliseconds < 0)
			{
				return;
			}

			var temp = cacheKey.ToJson();
			_cache.Add(cacheKey, value.ToJson(), ttl);
			//RedisHelper.Set(cacheKey, value.ToJson(), Convert.ToInt32(ttl.TotalSeconds));
		}

		public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
		{
			Add(key, value, ttl, region);
		}

		public void ClearRegion(string region)
		{
			var cacheKey = CacheKeyByRegion(region);
			_cache.Remove(cacheKey);
			//RedisHelper.Del(cacheKey);
		}

		public T Get(string key, string region)
		{
			T t = default(T);
			var cacheKey = GetCacheKey(region, key);
			var result = _cache.Get<T>(cacheKey);
			//var result = RedisHelper.Get(cacheKey);
			//if (!string.IsNullOrEmpty(result))
			//{
			//	t = result.ToObject<T>();
			//}

			return t;
		}

		public string GetCacheKey(string region, string key) => $"_options.CacheKeyPrefix-{region}-{key}";

		public string CacheKeyByRegion(string region) => $"_options.CacheKeyPrefix-{region}-*";

		private OcelotConfiguration _options;

		private ICache _cache;
	}

}
