using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using MicroAngels.Core;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace MicroAngels.Trace.Jaeger
{

	public class TracerProvider
	{

		public static ITracer GetTracer(string serviceName,ILoggerFactory loggerFactory,ISampler sampler,IReporter reporter)
		{			
			var tracer = new Tracer.Builder(serviceName);

			if (!loggerFactory.IsNull())
				tracer.WithLoggerFactory(loggerFactory);
			if (!sampler.IsNull())
				tracer.WithSampler(sampler);
			if (!reporter.IsNull())
				tracer.WithReporter(reporter);

			return tracer.Build();
		}
		
	}

	public class SamplerProvider
	{

		public static ISampler GetSampler(SamplerType type)
		{
			var defaultSampler = new ConstSampler(sample: true);

			switch (type)
			{
				case SamplerType.Const:
					return defaultSampler;
				default:
					return defaultSampler;
			}
			
		}

	}

	public class ReporterProvider
	{

		public static IReporter GetReporter(ReporterType type,ReporterOptions options)
		{
			var defaultReporter = new RemoteReporter.Builder()
			.WithSender(new UdpSender(options.RemoteHost, options.RemotePort, 0))
			.Build();

			switch (type)
			{
				case ReporterType.Remote:
					return defaultReporter;
				default:
					return defaultReporter;
			}

		}
	}

}
