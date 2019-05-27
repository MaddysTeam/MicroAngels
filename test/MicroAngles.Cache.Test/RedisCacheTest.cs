using MicroAngels.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace MicroAngles.Cache.Test
{

	public class RedisCacheTest
	{

		public RedisCacheTest()
		{
			_cache = new RedisCache(
				new RedisCacheOption("192.168.1.9", 6379, 0, 1000)
				);
		}

		[Fact]
		public void AddCacheTest()
		{
			var value = "value";
			var addResult = _cache.Add("key", value, TimeSpan.FromSeconds(1000));
			var result = _cache.Get<string>("key");

			Assert.True(addResult);
			Assert.Equal(value, result);
		}

		[Fact]
		public void AddCacheAndExpireTest()
		{
			var value = "value";
			_cache.Add("key", value, TimeSpan.FromSeconds(1));
			var result = _cache.Get<string>("key");

			Thread.Sleep(1000);

			Assert.NotEqual(value, result);
			Assert.Null(result);
		}

		[Fact]
		public void AddComplexCacheTest()
		{
			var value = new List<CacheObject> { new CacheObject { Id = 2 }, new CacheObject { Id = 3 } };
			var key = "key";
			var addResult = _cache.Add(key, value, TimeSpan.FromSeconds(2000));
			var result = _cache.Get<List<CacheObject>>(key);

			Assert.True(addResult);
		}

		[Fact]
		public void RemoveCacheTest()
		{
			var value = "value";
			var key = "key";
			_cache.Add(key, value, TimeSpan.FromSeconds(1000));

			var removeResult = _cache.Remove(key);
			var result = _cache.Get<string>(key);

			Assert.True(removeResult);
			Assert.Null(result);
		}


		private IRedisCache _cache;

		class CacheObject
		{
			public long Id { get; set; }
		}

	}

}
