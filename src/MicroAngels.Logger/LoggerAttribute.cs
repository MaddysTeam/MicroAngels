using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Logger
{

	public class LoggerAttribute:ActionFilterAttribute
	{

		protected readonly ILogger Logger;

		public LoggerAttribute(ILogger logger)
		{
			Logger = logger;
		}	
	
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
		}

	}


	//[ServiceFilter(typeof(DeleteSubionCache))]
}
