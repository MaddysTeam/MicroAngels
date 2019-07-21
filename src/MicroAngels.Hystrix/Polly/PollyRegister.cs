using AspectCore.Extensions.DependencyInjection;
using MicroAngels.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MicroAngels.Hystrix.Polly
{

	public class PollyRegister
	{

		public static IServiceProvider RegisterPollyServiceInAssembly(Assembly assembly, IServiceCollection services)
		{
			foreach(var type in assembly.ExportedTypes)
			{
				foreach(var method in type.GetMethods())
				{
					if (!method.GetCustomAttribute(typeof(PollyAttribute)).IsNull())
					{
						services.AddSingleton(type);
					}
				}
			}

			return services.BuildAspectInjectorProvider();
		}

	}

}
