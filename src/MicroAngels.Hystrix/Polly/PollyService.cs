using MicroAngels.Core;
using MicroAngels.Logger;
using Polly;
using Polly.Timeout;
using System;

namespace MicroAngels.Hystrix.Polly
{

	public class PollyService
	{

		public static ISyncPolicy Retry<TExecption>(int num, Action<Exception, int> callback) where TExecption : AngleExceptions
		{
			var policy = Policy.Handle<TExecption>().Retry(num, (e, i) =>
			  {
				  callback(e, i);
			  });

			return policy;
		}

		public static ISyncPolicy CircuitBreak<TException>(int allowedCountBeforeCirecuit,TimeSpan circuitDuration) where TException : AngleExceptions
		{
			var policy = Policy
			 .Handle<TException>()
			 .CircuitBreaker(allowedCountBeforeCirecuit, circuitDuration);

			return policy;
		}


		public static ISyncPolicy Timeout(TimeSpan timeoutDuration,Action fallback)
		{
			var timeoutExceptionPolicy = Policy.Handle<TimeoutRejectedException>().Fallback(fallback);
			var timeoutPolicy = Policy.Timeout(timeoutDuration, TimeoutStrategy.Pessimistic);

			var mainPolicy = Policy.Wrap(timeoutExceptionPolicy, timeoutPolicy);

			return mainPolicy;
		}

	}

}
