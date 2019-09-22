using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Trace.Jaeger
{

	public class JaegerOptions
	{
		public JaegerOptions()
		{
			SamplerType = SamplerType.Const;
			ReporterOptions = new ReporterOptions();
		}

		public string ServiceName { get; set; }
		public ILoggerFactory LoggerFactory{get;set;}
		public SamplerType SamplerType { get; set; }
		public ReporterType ReporterType { get; set; }
		public ReporterOptions ReporterOptions { get; set; }

	}

	public class ReporterOptions
	{
		public string RemoteHost { get; set; }
		public int RemotePort { get; set; }
	}

	public enum SamplerType
	{
		Default=0,
		Const=1
	}

	public enum ReporterType
	{
		Remote=1,
		Log=2
	}

}
