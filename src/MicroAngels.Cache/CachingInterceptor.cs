namespace MicroAngels.Cache
{
	using AspectCore.DynamicProxy;
	using AspectCore.Injector;
	using MicroAngels.Cache.Redis;
	using MicroAngels.Core;
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public class CachingInterceptor<T> : AbstractInterceptor
	{

		[FromContainer]
		public IRedisCache CacheProvider { get; set; }

		private char _linkChar = ':';

		public async override Task Invoke(AspectContext context, AspectDelegate next)
		{
			var cacheAttribute = GetAttribute(context.ServiceMethod);
			if (cacheAttribute != null)
			{
				await ExecuteCaching(context, next, cacheAttribute);
			}
			else
			{
				await next(context);
			}
		}

		private CachingAttribute GetAttribute(MethodInfo method)
		{
			return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
		}

		private async Task ExecuteCaching(AspectContext context, AspectDelegate next, CachingAttribute attribute)
		{
			// 生成缓存key 策略是 class + method + para values  使key既能唯一，又能方便用于删除
			var cacheKey = GenerateCacheKey(context);

			//如果是查询则先从缓存找，找到便中断方法
			if (attribute.ActionType == ActionType.search)
			{
				var cacheValue = CacheProvider.Get<T>(cacheKey);
				if (!cacheValue.IsNull())
				{
					if (attribute.IsAsync)
						context.ReturnValue = Task.FromResult(cacheValue);
					else
						context.ReturnValue = cacheValue;

					return;
				}
			}

			var deleteKeys = attribute.DeleteKeys;
			if (!deleteKeys.IsNull() && deleteKeys.Length > 0)
			{
				foreach (var key in deleteKeys)
				{
					if (CacheProvider.ExistKey(key))
						CacheProvider.Remove(key);
				}
			}

			// 执行方法体
			await next(context);

			// 执行方法体之后加入缓存
			if (!string.IsNullOrWhiteSpace(cacheKey))
			{
				var o = attribute.IsAsync ? (context.ReturnValue as Task<T>).Result : context.ReturnValue;
				//if (attribute.IsAsync)
				//	var returnValue = (context.ReturnValue as Task<T>).Result;

				CacheProvider.AddOrRemove(cacheKey, o, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
			}
		}

		protected virtual string GenerateCacheKey(AspectContext context)
		{
			var typeName = context.ServiceMethod.DeclaringType.Name;
			var methodName = context.ServiceMethod.Name;
			var arguments = context.Parameters;

			return this.GenerateCacheKey(typeName, methodName, arguments);
		}

		private string GenerateCacheKey(string typeName, string methodName, object[] arguments)
		{
			var builder = new StringBuilder();

			builder.Append(typeName);
			builder.Append(_linkChar);

			builder.Append(methodName);
			builder.Append(_linkChar);

			foreach (var param in arguments)
			{
				var paraStr = GetArgument(param);
				builder.Append(GetArgument(paraStr));
				builder.Append(_linkChar);
			}

			return builder.ToString().TrimEnd(_linkChar);
		}

		private string GetArgument(object arg)
		{
			if (arg is int || arg is long || arg is string || arg is Guid)
				return arg.ToString();

			if (arg is DateTime)
				return ((DateTime)arg).ToString("yyyyMMddHHmmss");

			if (arg is ICachable)
				return ((ICachable)arg).CacheKey;

			return string.Empty;
		}

	}

}
