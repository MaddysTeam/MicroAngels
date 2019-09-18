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
		}

		public string ServiceName { get; set; }
		public ILoggerFactory LoggerFactory{get;set;}
		public SamplerType SamplerType { get; set; }
	}

	public enum SamplerType
	{
		Const=1
	}

}
