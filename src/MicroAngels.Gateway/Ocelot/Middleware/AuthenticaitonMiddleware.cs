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
				if (_options.IsUseCustomAuthenticate)
				{
					var clientId = string.Empty;
					var clientClaim = context.HttpContext.User.Claims.FirstOrDefault();
					var path = context.DownstreamReRoute.UpstreamPathTemplate.OriginalValue;
					if (!string.IsNullOrEmpty(clientClaim?.Value))
					{
						// you can implement white or black list here
						clientId = clientClaim?.Value;
					}

					if (await _authenticateService.ValidateAuthenticate(clientId, path))
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
