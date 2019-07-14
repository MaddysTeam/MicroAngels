using CSRedis;
using System;
using System.Collections.Generic;
using MicroAngels.Core;
using System.Linq;

namespace MicroAngels.Cache.Redis
{

	public class RedisCache : IRedisCache
	{

		public RedisCache(RedisCacheOption option)
		{
			option.EnsureNotNull(() => new ArgumentNullException());

			_option = option;
			_client = new CSRedisClient(option.Conn);

			RedisHelper.Initialization(_client);
		}

		//public RedisCache(IEnumerable<RedisCacheOption> options)
		//{
		//	options.EnsureNotNull(() => new ArgumentNullException());

		//	var conn = string.Join(",", options.Select(x=>x.ToString()));
		//	_client = new CSRedisClient(conn);

		//	RedisHelper.Initialization(_client);
		//}


		public bool Add<T>(string key, T value, TimeSpan expire)
		{
			return RedisHelper.Set(key, value, (int)expire.TotalSeconds);
		}

		public bool AddOrRemove<T>(string key, T value, TimeSpan expire)
		{
			var isExist = Get<T>(key) == null ? true : false;
			if (isExist)
				Remove(key);

			return Add(key, value, expire);
		}

		public T Get<T>(string key)
		{
			return RedisHelper.Get<T>(key);
		}

		public void Refresh(string key, TimeSpan expire)
		{
			RedisHelper.Expire(key, (int)expire.TotalSeconds);
		}

		public bool Remove(string key)
		{
			var res= RedisHelper.Del(key);
			return res >= 0;
		}

		public bool ExistKey(string key)
		{
			return RedisHelper.Exists(key);
		}

		public IEnumerable<string> GetAllKey()
		{
			return RedisHelper.Keys("*");
		}

		private readonly CSRedisClient _client;
		private readonly RedisCacheOption _option;

	}

}
