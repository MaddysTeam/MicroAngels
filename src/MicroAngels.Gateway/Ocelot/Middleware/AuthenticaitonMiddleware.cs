using MicroAngels.Core;
using Microsoft.AspNetCore.Http;
using Ocelot.Logging;
using Ocelot.Middleware;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public class AuthenticaitonMiddleware : OcelotMiddleware
	{

		public AuthenticaitonMiddleware(OcelotRequestDelegate next,
			IOcelotLoggerFactory loggerFactory,
			ICustomAuthenticateService authenticateService,
			OcelotConfiguration options)
			: base(loggerFactory.CreateLogger<AuthenticaitonMiddleware>())
		{
			_next = next;
			_authenticateService = authenticateService;
			_options = options;
		}


		public async Task Invoke(DownstreamContext context)
		{
			if (!context.IsError &&
				 context.HttpContext.Request.Method.ToUpper() != "OPTIONS" &&
				 context.DownstreamReRoute.IsAuthenticated
				 )
			{
				if (_options.IsUseCustomAuthenticate && !_authenticateService.IsNull())
				{
					var path = context.DownstreamReRoute.UpstreamPathTemplate.OriginalValue;
					var httpContext = context.HttpContext;
					if (await _authenticateService.ValidateAuthenticate(httpContext, path))
					{
						await _next.Invoke(context);
					}
					else
					{
						// var error = new ErrorResult();
						var message = "";
						await context.HttpContext.Response.WriteAsync(message);
					}
				}
				else
				{
					await _next.Invoke(context);
				}

			}
		}

		private readonly OcelotRequestDelegate _next;
		private readonly OcelotConfiguration _options;
		private readonly ICustomAuthenticateService _authenticateService;

	}

}
