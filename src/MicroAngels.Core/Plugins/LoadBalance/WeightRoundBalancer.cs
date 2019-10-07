using System;
using System.Collections.Generic;

namespace MicroAngels.Core.Plugins
{

	/// <summary>
	/// 加权随机均衡器
	/// </summary>
	public class WeightRoundBalancer : ILoadBalancer
	{
		public string Name => "WeightRound";

		private static Random _rand = new Random();

		public T Balance<T>(IDictionary<T, int> source)
		{
			if (source == null || source.Count == 0)
				return default(T);

			var tmp = new List<T>();
			foreach (var key in source.Keys)
			{
				var weight = source[key];
				for (int i = 0; i <= weight; i++)
				{
					tmp.Add(key);
				}
			}

			var next = _rand.Next(tmp.Count - 1);
			//if (next > tmp.Count)
			//	next = tmp.Count - 1;

			return tmp[next];
		}
	}

}
