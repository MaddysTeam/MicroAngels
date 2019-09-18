using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
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

}
