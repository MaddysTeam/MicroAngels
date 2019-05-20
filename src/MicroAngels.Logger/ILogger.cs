using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Logger
{

	public interface ILogger
	{
		void Error(string msg,string[] args,Exception e);
		void Critical(string msg,string[] args, Exception e);
		void Warning(string msg,string[] args);
		void Info(string msg,string[] args);
	}


}
