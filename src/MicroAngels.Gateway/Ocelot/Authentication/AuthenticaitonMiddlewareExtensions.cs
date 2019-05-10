﻿using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public static class AuthenticaitonMiddlewareExtensions 
	{

		public static IOcelotPipelineBuilder UseAuthenticationMiddleware(this IOcelotPipelineBuilder builder)
		{
			return builder.UseMiddleware<AuthenticaitonMiddleware>();
		}

	}

}
