using MicroAngels.Core;
using Microsoft.AspNetCore.Authentication;
using Ocelot.Configuration.Creator;
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
					
					var result = await context.HttpContext.AuthenticateAsync(context.DownstreamReRoute.AuthenticationOptions.AuthenticationProviderKey);
					if (result.Succeeded)
					{
						context.HttpContext.User = result.Principal;
					}

					await _next.Invoke(context);

					//if (await _authenticateService.ValidateAuthenticate(context))
					//{
					//	await _next.Invoke(context);
					//}
					//else
					//{
					//	await context.HttpContext.Response.SendForbiddenReponse("application/Json", new { message = "permission deny" }.ToJson());
					//}
				}
				else
					await _next.Invoke(context);
			}
		}

		private readonly OcelotRequestDelegate _next;
		private readonly OcelotConfiguration _options;
		private readonly ICustomAuthenticateService _authenticateService;

	}

}
