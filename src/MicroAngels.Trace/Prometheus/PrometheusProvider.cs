using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace MicroAngels.Trace.Prometheus
{

	public static  class PrometheusProvider
	{

		public static IApplicationBuilder UsePrometheus(this IApplicationBuilder builder)
		{
			return builder.UseMetricServer();
		}

	}

}
