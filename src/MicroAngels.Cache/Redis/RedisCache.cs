using CSRedis;
using System;

namespace MicroAngels.Cache.Redis
{

	class RedisCache<T> : IRedisCache<T>
	{

		public RedisCache(RedisCacheOption option)
		{
			_option = option;
			_client = new CSRedisClient(option.Conn);

			RedisHelper.Initialization(_client);
		}


		public void Add(string key, T value, TimeSpan expire)
		{
			//_client=new CSRedisClient()
			var result = RedisHelper.Set(key, value, (int)expire.TotalSeconds);
			if (!result)
			{

			}
		}

		public void AddOrRemove(string key, T value, TimeSpan expire)
		{
			var isExist = Get(key) == null ? true : false;
			if (isExist)
				Remove(key);

			Add(key, value, expire);
		}

		public T Get(string key)
		{
			return RedisHelper.Get<T>(key);
		}

		public TimeSpan Refresh(string key, TimeSpan expire)
		{
			//TODO:
			return TimeSpan.FromDays(0);
		}

		public bool Remove(string key)
		{
			return true;
			//return RedisHelper.Expire(key);
		}

		private CSRedisClient _client;
		private RedisCacheOption _option;

	}

}
