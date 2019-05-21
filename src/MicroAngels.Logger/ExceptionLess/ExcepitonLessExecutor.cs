using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using MicroAngels.Core;

namespace MicroAngels.Logger.ExceptionLess
{

	public class ExcepitonLessExecutor : IFilterLogExecutor
	{

		public ExcepitonLessExecutor(ILogger logger)
		{
			logger.EnsureNotNull(() => new ArgumentNullException());

			_logger = logger;
		}

		public void Execute<Context>(Context ctx) where Context : FilterContext
		{
			_logger.Info($"controller:{ctx.ActionDescriptor.DisplayName}",null);
		}

		public Task ExecuteAsync<Context>(Context ctx) where Context : FilterContext
		{
			throw new NotImplementedException();
		}

		private readonly ILogger _logger;

	}

}
