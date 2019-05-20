using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Plugins
{

	public interface ILoadBalancer
	{
		string Name { get; }

		T Balance<T>(IDictionary<T, int> source);
	}

}
