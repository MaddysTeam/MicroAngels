using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{

    public class ActionLoggerFilter : ActionFilterAttribute
    {

        private readonly ILogger<ActionLoggerFilter> _logger;

        public ActionLoggerFilter(ILogger<ActionLoggerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogTrace($"Action named {context.ActionDescriptor.DisplayName} excuted start");

            base.OnActionExecuted(context);

            _logger.LogTrace($"Action named {context.ActionDescriptor.DisplayName} excuted end");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogTrace($"Action named {context.ActionDescriptor.DisplayName} excuting start");

            base.OnActionExecuting(context);

            _logger.LogTrace($"Action named {context.ActionDescriptor.DisplayName} excuted end");
        }

    }

}
