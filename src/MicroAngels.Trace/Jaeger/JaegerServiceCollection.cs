using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
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


			ISampler sampler = null;
			if (options.SamplerType == SamplerType.Const)
				sampler = new ConstSampler(sample: true);

			var reporter = new RemoteReporter.Builder()
				.WithSender(new UdpSender("192.168.1.9", 6831, 0))
				.Build();

			var tracer = TracerProvider.GetTracer(options.ServiceName, options.LoggerFactory, sampler, reporter);
			GlobalTracer.Register(tracer);
			serivces.AddSingleton(tracer);

			serivces.AddOpenTracing();

			return serivces;
		}

	}

}
