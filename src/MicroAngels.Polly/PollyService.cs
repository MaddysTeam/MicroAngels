using MicroAngels.Core;
using MicroAngels.Logger;
using Polly;
using System;

namespace MicroAngels.Polly
{

	public class PollyService
	{

		public PollyService(ILogger logger)
		{
			_logger = logger;
		}

		//public PollyService Retry<TExecption>(int num,Action<Exception,int> callback) where TExecption : AngleExceptions
		//{
		//	var policy=Policy.Handle<TExecption>().Retry(num, (e, i) =>
		//	{
		//		callback(e, i);
		//	});

		//	return this;
		//}

		//public PollyService	 RetryWarpAsync<TException>(int num) where TException : AngleExceptions
		//{
		//	var policy = Policy
		//	 .Handle<TException>()
		//	 .RetryAsync(num, (ex, count) =>
		//	 {
		//	 });

		//	return this;
		//}

		public static AsyncPolicy CircuitBreakAsync<TException>(int allowedCountBeforeCirecuit,TimeSpan circuitDuration) where TException : AngleExceptions
		{
			var policy = Policy
			 .Handle<TException>()
			 .CircuitBreakerAsync(allowedCountBeforeCirecuit, circuitDuration);

			return policy;
		}



		private readonly ILogger _logger;

	}

}
