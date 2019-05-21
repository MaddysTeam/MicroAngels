using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Logger
{

	public interface ILogger
	{
		void Error(string msg,string[] args,Exception e=null);
		void Critical(string msg,string[] args, Exception e=null);
		void Warning(string msg,string[] args);
		void Info(string msg,string[] args);
	}


}
