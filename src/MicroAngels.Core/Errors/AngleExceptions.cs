using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core
{

	public class AngleExceptions:Exception
	{

		public AngleExceptions(string message):base(message) { }

		public AngleExceptions(string message,Exception inner) : base(message,inner) { }

	}

}
