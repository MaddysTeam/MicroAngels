using System;
using System.Collections.Generic;
using System.Text;
using Exceptionless;
using Exceptionless.AspNetCore;
using Exceptionless.Logging;


namespace MicroAngels.Logger.ExceptionLess
{

	public class ExceptionLessLogger : ILogger
	{

		public void Critical(string msg, string[] args, Exception e)
		{
			ExceptionlessClient.Default.CreateLog(msg, LogLevel.Fatal)
				.AddTags(args)
				.Submit();
		}

		public void Error(string msg, string[] args, Exception e)
		{
			throw new NotImplementedException();
		}

		public void Info(string msg, string[] args)
		{
			throw new NotImplementedException();
		}

		public void Warning(string msg, string[] args)
		{
			throw new NotImplementedException();
		}

	}

}
