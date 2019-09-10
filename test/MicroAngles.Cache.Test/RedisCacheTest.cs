using MicroAngels.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroAngles.Cache.Test
{

	public class RedisCacheTest
	{

		public RedisCacheTest()
		{
			_cache = new RedisCache(
				new RedisCacheOption("127.0.0.1", 6379, 0, 1000)
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

		[Fact]
		public void LockTest()
		{
			var lockKey = "lockkey";
			var timeout = TimeSpan.FromMilliseconds(3000);
			var tasks = new List<Task>();
			var results = new List<bool>();
			for (int i = 0; i < 3; i++)
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					results.Add(_cache.Lock(lockKey, timeout));
				}));
			}

			Task.WaitAll(tasks.ToArray());

			Assert.True(results.FindAll(x => x == false).Count == 2);
		}


		private IRedisCache _cache;

		class CacheObject
		{
			public long Id { get; set; }
		}

	}

}
