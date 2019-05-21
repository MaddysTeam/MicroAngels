using Exceptionless;
using Exceptionless.Logging;
using System;


namespace MicroAngels.Logger.ExceptionLess
{

	public class ExceptionLessLogger : ILogger
	{

		public void Critical(string msg, string[] args, Exception e=null)
		{
			ExceptionlessClient.Default.CreateLog(msg, LogLevel.Fatal).AddTags(args).Submit();
		}

		public void Error(string msg, string[] args, Exception e = null)
		{
			ExceptionlessClient.Default.CreateLog(msg, LogLevel.Error).AddTags(args).Submit() ;
		}

		public void Info(string msg, string[] args)
		{
			ExceptionlessClient.Default.CreateLog(msg, LogLevel.Info).AddTags(args).Submit();
		}

		public void Warning(string msg, string[] args)
		{
			ExceptionlessClient.Default.CreateLog(msg, LogLevel.Warn).AddTags(args).Submit();
		}

	}

}
