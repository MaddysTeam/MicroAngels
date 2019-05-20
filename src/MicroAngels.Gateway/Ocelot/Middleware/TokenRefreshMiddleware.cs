using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public class TokenRefreshMiddleware : OcelotMiddleware
	{

		public TokenRefreshMiddleware(OcelotRequestDelegate next,
			IOcelotLoggerFactory loggerFactory,
			OcelotConfiguration options)
			: base(loggerFactory.CreateLogger<AuthenticaitonMiddleware>())
		{
			_next = next;
			_options = options;
		}


		public async Task Invoke(DownstreamContext context)
		{
			if (!context.IsError &&
				 context.HttpContext.Request.Method.ToUpper() != "OPTIONS" &&
				 context.DownstreamReRoute.IsAuthenticated
				 )
			{
				if (_options.TokenRefreshIterval != null)
				{
					
				}
			}

			await _next.Invoke(context);
		}

		private readonly OcelotRequestDelegate _next;
		private readonly OcelotConfiguration _options;


	}

}
