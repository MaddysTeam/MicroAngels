using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Polly
{

	public class PollyOptoins
	{
		public int RetryCount { get; set; }

		public TimeSpan CircuitTimeout { get; set; }
	}

	public class PollyExceptionOptions { }

	public class PollyRetryOptions { }

	public class PollyCirecuitOptions { }

	public class PollyTimeoutOptons { }

}
