using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;


namespace MicroAngels.Core
{

	public static class ServiceProviderExtensions
	{

		public static IOptionsMonitor<T> GetMonitor<T>(this IServiceProvider serviceProvider,Action<T,string> onChange) where T : class
		{
			var monitor= serviceProvider.GetService<IOptionsMonitor<T>>();
			if (!onChange.IsNull())
			{
				monitor.OnChange(onChange);
			}

			return monitor;
		}
	}
}
