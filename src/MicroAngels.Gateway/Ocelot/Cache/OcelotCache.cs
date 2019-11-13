using MicroAngels.Cache;
using MicroAngels.Core;
using Ocelot.Cache;
using System;

namespace MicroAngels.Gateway.Ocelot
{

	public class OcelotCache<T> : IOcelotCache<T>
	{

		public OcelotCache(OcelotConfiguration options,ICache cache)
		{
			_options = options;
			_cache = cache;
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
		}

		public T Get(string key, string region)
		{
			var cacheKey = GetCacheKey(region, key);
			var result = _cache.Get<T>(cacheKey);

			return result;
		}

		public string GetCacheKey(string region, string key) => $"_options.CacheKeyPrefix-{region}-{key}";

		public string CacheKeyByRegion(string region) => $"_options.CacheKeyPrefix-{region}-*";

		private OcelotConfiguration _options;

		private ICache _cache;
	}

}
