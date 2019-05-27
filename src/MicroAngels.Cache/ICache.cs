using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Cache
{

	public interface ICache
	{
		bool Add<T>(string key,T value,TimeSpan expire);
		bool AddOrRemove<T>(string key,T value,TimeSpan expire);
		T Get<T>(string key);
		bool Remove(string key);
		void Refresh(string key,TimeSpan expire);
	}

}
