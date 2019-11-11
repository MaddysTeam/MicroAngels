using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using MicroAngels.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MicroAngels.Hystrix.Polly
{

	public class PollyRegister
	{

		public static IServiceProvider RegisterPollyServiceInAssembly(Assembly assembly, IServiceCollection services,IConfiguration configuration)
		{
			services.ToServiceContainer().AddInstance(configuration);

			foreach (var type in assembly.ExportedTypes)
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
