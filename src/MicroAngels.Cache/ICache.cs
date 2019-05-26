using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Cache
{

	public interface ICache<T>
	{
		void Add(string key,T value,TimeSpan expire);
		void AddOrRemove(string key,T value,TimeSpan expire);
		T Get(string key);
		bool Remove(string key);
		TimeSpan Refresh(string key,TimeSpan expire);
	}

}
