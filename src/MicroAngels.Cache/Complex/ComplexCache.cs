using MicroAngels.Cache.Redis;
using MicroAngels.Core;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace MicroAngels.Cache.Complex
{

	public class ComplexCache : IComplexCache, ICache
	{

		public ComplexCache(IMemoryCache memoryCache, IRedisCache redisCache)
		{
			memoryCache.EnsureNotNull(() => new AngleExceptions());
			redisCache.EnsureNotNull(() => new AngleExceptions());

			_memoryCache = memoryCache;
			_redisCache = redisCache;
		}

		public bool Add<T>(string key, T value, TimeSpan expire)
		{
			return _redisCache.Add(key, value, expire) || _memoryCache.Set(key, value, expire).IsNull();
		}

		public bool AddOrRemove<T>(string key, T value, TimeSpan expire)
		{
			if (Remove(key))
				return Add(key, value, expire);

			return false;
		}

		public T Get<T>(string key)
		{
			return _redisCache.Get<T>(key) == null ? _memoryCache.Get<T>(key) : default(T);
		}

		public bool Remove(string key)
		{
			try
			{
				_memoryCache.Remove(key);
				return _redisCache.Remove(key);
			}
			catch
			{
				return false;
			}
		}

		public void Refresh(string key, TimeSpan expire)
		{
			throw new NotImplementedException();
		}

		public bool ExistKey(string key)
		{
			return _redisCache.ExistKey(key) || !_memoryCache.Get(key).IsNull();
		}

		public IEnumerable<string> GetAllKey()
		{
			throw new NotImplementedException();
		}

		private IMemoryCache _memoryCache;
		private IRedisCache _redisCache;

	}

}
