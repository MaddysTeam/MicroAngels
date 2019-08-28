using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Plugins
{

	/// <summary>
	/// 加权随机均衡器
	/// </summary>
	public class WeightRoundBalancer : ILoadBalancer
	{
		public string Name => "WeightRound";

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

			var rand = new Random(tmp.Count);
			var next = rand.Next();
			if (next > tmp.Count)
				next = tmp.Count - 1;

			return tmp[next];
		}
	}

}
