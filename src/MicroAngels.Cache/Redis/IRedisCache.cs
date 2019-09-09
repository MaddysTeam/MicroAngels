using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Cache.Redis
{

	public interface IRedisCache:ICache
	{
		bool Lock(string lockKey,TimeSpan expire);
		bool Unlock(string lockKey);
	}

}
