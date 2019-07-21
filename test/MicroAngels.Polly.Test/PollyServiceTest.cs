using MicroAngels.Core;
using MicroAngels.Hystrix.Polly;
using System;
using Xunit;

namespace MicroAngels.Polly.Test
{

	public class PollyServiceTest
	{


		[Fact]
		public void InvokePollyAttributeTest()
		{

		}

		[Fact]
		public void PollyRetryTset()
		{
			var policy = PollyService.Retry<MicroAngels.Core.AngleExceptions>(_retryCount, (e, n) =>
			{
				Console.WriteLine(n);
				Console.WriteLine(e.ToString());
				_retryCount--;
			});

			try
			{
				policy.Execute(() =>
				{
					Console.WriteLine("Job Start");
					if (DateTime.Now.Second % 10 != 0)
					{
						throw new AngleExceptions("Special error occured");
					}
					Console.WriteLine("Job End");
				});
			}
			catch
			{
				Assert.True(_retryCount == 0);
			}
		}

		[Fact]
		public void CircuitBreakTest()
		{
			var policy = PollyService.CircuitBreak<AngleExceptions>(_retryCount, TimeSpan.FromSeconds(10));

			try
			{
				while (true)
				{
					try
					{
						policy.Execute(() =>
						{
							_retryCount--;
							Console.WriteLine("Job Start");
							throw new AngleExceptions("Special error occured");
							Console.WriteLine("Job End");
						});
					}
					catch (AngleExceptions ex)
					{
						Console.WriteLine("There's one unhandled exception : " + ex.Message);
					}

					System.Threading.Thread.Sleep(500);
				}
			}
			catch
			{
				Assert.True(_retryCount == 0);
			}
		}

		[Fact]
		public void TimeoutTest()
		{
			var policy = PollyService.Timeout(TimeSpan.FromSeconds(5),
				() =>
				{
					System.Console.WriteLine("time out");
				});

			try
			{
				policy.Execute(()=> {
					Console.WriteLine("Job Start");
					System.Threading.Thread.Sleep(10000);
					Console.WriteLine("Job Stop");
				});
			}
			catch
			{
				System.Console.WriteLine("gone");
			}
		}

		private static int _retryCount = 2;
	}

}
