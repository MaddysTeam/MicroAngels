using Ocelot.Cache;
using Ocelot.Logging;
using Ocelot.Middleware;
using System;
using System.Threading.Tasks;
using System.Linq;
using MicroAngels.Core;

namespace MicroAngels.Gateway.Ocelot
{

	public class TokenRefreshMiddleware : OcelotMiddleware
	{

		public TokenRefreshMiddleware(OcelotRequestDelegate next,
			IOcelotLoggerFactory loggerFactory,
			ICustomTokenRefreshService refreshService,
			OcelotConfiguration options)
			: base(loggerFactory.CreateLogger<AuthenticaitonMiddleware>())
		{
			_next = next;
			_options = options;
			_tokenRefreshService = refreshService;
		}


		public async Task Invoke(DownstreamContext context)
		{
			if (!context.IsError &&
				 context.HttpContext.Request.Method.ToUpper() != "OPTIONS" &&
				 context.DownstreamReRoute.IsAuthenticated
				 )
			{
				await _tokenRefreshService.Refresh(context);
			}

			await _next.Invoke(context);
		}

		private readonly OcelotRequestDelegate _next;
		private readonly OcelotConfiguration _options;
		private readonly ICustomTokenRefreshService _tokenRefreshService;

	}

}
