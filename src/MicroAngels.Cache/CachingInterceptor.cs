namespace MicroAngels.Cache
{
    using AspectCore.DynamicProxy;
    using AspectCore.Injector;
	using MicroAngels.Cache.Redis;
	using MicroAngels.Core;
	using System;
    using System.Collections.Generic;
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
            var qCachingAttribute = GetQCachingAttributeInfo(context.ServiceMethod);
            if (qCachingAttribute != null)
            {
                await ProceedCaching(context, next, qCachingAttribute);
            }
            else
            {
                await next(context);
            }
        }


        private CachingAttribute GetQCachingAttributeInfo(MethodInfo method)
        {
            return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
        }


        private async Task ProceedCaching(AspectContext context, AspectDelegate next, CachingAttribute attribute)
        {
            var cacheKey = GenerateCacheKey(context);

			if (attribute.ActionType==ActionType.search)
			{
				var cacheValue = CacheProvider.Get<T>(cacheKey);
				if (cacheValue != null)
				{
					context.ReturnValue = cacheValue;
					return;
				}
			}

            await next(context);

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CacheProvider.AddOrRemove(cacheKey, context.ReturnValue, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
            }
        }


        protected virtual string GenerateCacheKey(AspectContext context)
        {
            var typeName = context.ServiceMethod.DeclaringType.Name;
            var methodName = context.ServiceMethod.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(context.ServiceMethod.GetParameters());

			var parms = context.Parameters;

            return this.GenerateCacheKey(typeName, methodName, methodArguments);
        }

        private string GenerateCacheKey(string typeName, string methodName, IList<string> parameters)
        {
            var builder = new StringBuilder();

            builder.Append(typeName);
            builder.Append(_linkChar);

            builder.Append(methodName);
            builder.Append(_linkChar);

            foreach (var param in parameters)
            {
                builder.Append(param);
                builder.Append(_linkChar);
            }

            return builder.ToString().TrimEnd(_linkChar);
        }

        private IList<string> FormatArgumentsToPartOfCacheKey(IList<ParameterInfo> methodArguments, int maxCount = 5)
        {
            return methodArguments.Select(this.GetArgumentValue).Take(maxCount).ToList();
        }

		private string GetArgumentValue(object arg)
		{
			if (arg is int || arg is long || arg is string)
				return arg.ToString();

			if (arg is DateTime)
				return ((DateTime)arg).ToString("yyyyMMddHHmmss");

			if (arg is ICachable)
				return ((ICachable)arg).CacheKey;

			return null;
		}

	}

}
