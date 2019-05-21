using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroAngels.Logger.ExceptionLess
{

	public class DefaultLoggerExecutor : IFilterLogExecutor
	{

		public void Execute<Context>(Context ctx) where Context : FilterContext
		{
			throw new NotImplementedException();
		}

		public Task ExecuteAsync<Context>(Context ctx) where Context : FilterContext
		{
			throw new NotImplementedException();
		}

	}

}
