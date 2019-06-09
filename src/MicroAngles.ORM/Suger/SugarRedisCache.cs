using System;
using System.Collections.Generic;
using MicroAngels.Cache.Redis;
using SqlSugar;

namespace MicroAngels.ORM.Suger
{

	public class SugarRedisCache : ICacheService
	{
		
		public SugarRedisCache(IRedisCache service)
		{
			_service = service;
		}

		public void Add<V>(string key, V value)
		{
			_service.Add(key, value,TimeSpan.MaxValue);
		}

		public void Add<V>(string key, V value, int cacheDurationInSeconds)
		{
			_service.Add(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
		}

		public bool ContainsKey<V>(string key)
		{
			return _service.ExistKey(key);
		}

		public V Get<V>(string key)
		{
			return _service.Get<V>(key);
		}

		public IEnumerable<string> GetAllKey<V>()
		{
			return _service.GetAllKey();
		}

		public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
		{
			if (this.ContainsKey<V>(cacheKey))
			{
				return this.Get<V>(cacheKey);
			}
			else
			{
				var result = create();
				this.Add(cacheKey, result, cacheDurationInSeconds);
				return result;
			}
		}

		public void Remove<V>(string key)
		{
			_service.Remove(key);
		}

		private readonly IRedisCache _service;

	}

}
