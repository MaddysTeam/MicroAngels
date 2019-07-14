using AspectCore.DynamicProxy;
using MicroAngels.Core;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace MicroAngels.Polly
{

	/// <summary>
	/// 基于polly 的 AOP 拦截器
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class PollyAttribute : AbstractInterceptorAttribute
	{

		public string DowngradeMethod { get; set; }

		public bool EnableCircuitBroken { get; set; }

		public int AllowedCountBeforeCirecuit { get; set; }

		public TimeSpan CircuitDuration { get; set; }

		public override async Task Invoke(AspectContext context, AspectDelegate next)
		{
			policies.TryGetValue(context.ServiceMethod, out AsyncPolicy policy);

			lock (policies)
			{

				if (policies.IsNull())
				{
					policy = Policy.NoOpAsync();

					if (EnableCircuitBroken)
					{
						//policy = PollyService.CircuitBreak<AngleExceptions>(AllowedCountBeforeCirecuit, CircuitDuration);
					}

					var fallbackPolicy = Policy.Handle<AngleExceptions>().FallbackAsync(async (ctx, c) =>
					 {
						 var acontext = ctx["acontext"] as AspectContext;
						 var downgradeMethod = acontext.ServiceMethod.DeclaringType.GetMethod(DowngradeMethod);
						 var result = downgradeMethod.Invoke(context.Implementation, context.Parameters);
						 acontext.ReturnValue = result;
					 },  (ex, t) => { return Task.Run(null); });

					policy = fallbackPolicy.WrapAsync(policy);
					policies.TryAdd(context.ServiceMethod, policy);
				}
			}

			var pollyContext = new Context();
			pollyContext["acontext"] = context;

			await policy.ExecuteAsync(ctx => next(context), pollyContext);
		}

		private static ConcurrentDictionary<MethodInfo, AsyncPolicy> policies = new ConcurrentDictionary<MethodInfo, AsyncPolicy>();

	}

}
