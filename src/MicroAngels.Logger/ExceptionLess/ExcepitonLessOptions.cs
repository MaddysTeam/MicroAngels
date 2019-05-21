using Exceptionless;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Logger.ExceptionLess
{

	public class ExcepitonLessOptions
	{

		public ExcepitonLessOptions(string appkey)
		{
			Appkey = appkey;
		}

		public readonly string Appkey;

	}

}
