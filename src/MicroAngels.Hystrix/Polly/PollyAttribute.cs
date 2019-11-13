using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Timeout;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace MicroAngels.Hystrix.Polly
{

	[AttributeUsage(AttributeTargets.Method)]
	public class PollyAttribute : AbstractInterceptorAttribute
	{
		/// <summary>
		/// retry count
		/// </summary>
		public int MaxRetryTimes { get; set; } = 0;

		/// <summary>
		/// retry interval seconds
		/// </summary>
		public int RetryIntervalMilliseconds { get; set; } = 100;

		/// <summary>
		/// is enable circuit breaker
		/// </summary>
		public bool IsEnableCircuitBreaker { get; set; } = false;

		/// <summary>
		/// allowed times before throw exception
		/// </summary>
		public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;

		/// <summary>
		/// circuit break duration
		/// </summary>
		public int MillisecondsOfBreak { get; set; } = 1000;

		/// <summary>
		/// time out allowed seconds
		/// </summary>
		public int TimeOutMilliseconds { get; set; } = 0;

		/// <summary>
		/// cache ttl seconds
		/// </summary>
		public int CacheTTLMilliseconds { get; set; } = 0;

		/// <summary>
		/// is force downgrade
		/// </summary>
		public bool IsForceDowngrade { get; set; } = false;

		[FromContainer]
		protected IConfiguration Configuration { get; set; }

		private static ConcurrentDictionary<MethodInfo, AsyncPolicy> policies
			= new ConcurrentDictionary<MethodInfo, AsyncPolicy>();

		private static readonly IMemoryCache memoryCache
			= new MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());

		/// <summary>
		/// HystrixCommandAttribute
		/// </summary>
		/// <param name="fallBackMethod">downgrade method</param>
		public PollyAttribute(string fallBackMethod)
		{
			FallBackMethod = fallBackMethod;
		}

		public string FallBackMethod { get; set; }

		public override async Task Invoke(AspectContext context, AspectDelegate next)
		{
			//一个HystrixCommand中保持一个policy对象即可
			//其实主要是CircuitBreaker要求对于同一段代码要共享一个policy对象
			//根据反射原理，同一个方法的MethodInfo是同一个对象，但是对象上取出来的HystrixCommandAttribute
			//每次获取的都是不同的对象，因此以MethodInfo为Key保存到policies中，确保一个方法对应一个policy实例
			policies.TryGetValue(context.ServiceMethod, out AsyncPolicy policy);
			lock (policies)//因为Invoke可能是并发调用，因此要确保policies赋值的线程安全
			{
				if (policy == null)
				{
					policy = Policy.NoOpAsync();//创建一个空的Policy
					if (IsEnableCircuitBreaker)
					{
						policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(IsForceDowngrade ? 1 : ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
					}
					if (TimeOutMilliseconds > 0)
					{
						policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), TimeoutStrategy.Pessimistic));
					}
					if (MaxRetryTimes > 0)
					{
						policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds)));
					}

					AsyncPolicy policyFallBack = Policy
					.Handle<Exception>()
					.FallbackAsync(async (ctx, t) =>
					{
						var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
						Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
						//不能如下这样，因为这是闭包相关，如果这样写第二次调用Invoke的时候context指向的
						//还是第一次的对象，所以要通过Polly的上下文来传递AspectContext
						//context.ReturnValue = fallBackResult;
						AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
						aspectContext.ReturnValue = fallBackResult;
					}, async (ex, t) =>
					{
						t.Add("error", ex.Message);
					});

					policy = policyFallBack.WrapAsync(policy);
					//放入
					policies.TryAdd(context.ServiceMethod, policy);
				}
			}

			//把本地调用的AspectContext传递给Polly，主要给FallbackAsync中使用，避免闭包的坑
			Context pollyCtx = new Context();
			pollyCtx["aspectContext"] = context;

			// 强制降级
			if (IsForceDowngrade)
			{
				await policy.ExecuteAsync((i) => throw new Core.AngleExceptions("force downgrade"), pollyCtx);

				return;
			}

			//Install-Package Microsoft.Extensions.Caching.Memory
			if (CacheTTLMilliseconds > 0)
			{
				//用类名+方法名+参数的下划线连接起来作为缓存key
				string cacheKey = "HystrixMethodCacheManager_Key_" + context.ServiceMethod.DeclaringType
																   + "." + context.ServiceMethod + string.Join("_", context.Parameters);
				//尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
				if (memoryCache.TryGetValue(cacheKey, out var cacheValue))
				{
					context.ReturnValue = cacheValue;
				}
				else
				{
					//如果缓存中没有，则执行实际被拦截的方法
					await policy.ExecuteAsync(ctx => next(context), pollyCtx);
					//存入缓存中
					using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
					{
						cacheEntry.Value = context.ReturnValue;
						cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(CacheTTLMilliseconds);
					}
				}
			}
			else//如果没有启用缓存，就直接执行业务方法
			{
				await policy.ExecuteAsync(ctx => next(context), pollyCtx);
			}
		}
	}

}
