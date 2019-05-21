using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MicroAngels.Logger
{

	public class LoggerAttribute : ActionFilterAttribute
	{

		public LoggerAttribute(IFilterLogExecutor executor)
		{
			executor.EnsureNotNull(() => new ArgumentNullException());
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

		private readonly IFilterLogExecutor _executor;

	}

	//[ServiceFilter(typeof(DeleteSubionCache))]
}
