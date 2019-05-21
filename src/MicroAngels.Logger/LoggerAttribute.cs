using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MicroAngels.Logger
{

	public class LoggerAttribute : ActionFilterAttribute
	{

		private readonly ILogger _logger;
		private readonly IFilterLogExecutor _executor;

		public LoggerAttribute(ILogger logger, IFilterLogExecutor executor)
		{
			logger.EnsureNotNull(() => new ArgumentNullException());
			executor.EnsureNotNull(() => new ArgumentNullException());

			_logger = logger;
			_executor = executor;
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			_executor.Execute(context);

			base.OnActionExecuted(context);
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			_executor.Execute(context);

			base.OnActionExecuting(context);
		}

	}


	//[ServiceFilter(typeof(DeleteSubionCache))]
}
