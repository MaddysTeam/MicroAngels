using MicroAngels.Core;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing.Util;
using System;

namespace MicroAngels.Trace.Jaeger
{

	public static class JaegerServiceCollection
	{

		public static IServiceCollection AddJaegerTrace(this IServiceCollection serivces, Action<JaegerOptions> settings)
		{
			var options = new JaegerOptions();		
			if (!settings.IsNull())
				settings(options);

			if (options.ServiceName.IsNullOrEmpty())
				throw new AngleExceptions("service name cannot be null or empty");

			var sampler = SamplerProvider.GetSampler(options.SamplerType);
			var reporter = ReporterProvider.GetReporter(options.ReporterType, options.ReporterOptions);
			var tracer = TracerProvider.GetTracer(options.ServiceName, options.LoggerFactory, sampler, reporter);
			GlobalTracer.Register(tracer);
			serivces.AddSingleton(tracer);
			serivces.AddOpenTracing();

			return serivces;
		}

	}
	
}
