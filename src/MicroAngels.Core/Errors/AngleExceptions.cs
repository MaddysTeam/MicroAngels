using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core
{

	public class AngleExceptions : Exception, IError
	{

		public AngleExceptions() { }

		public AngleExceptions(string message) : base(message) { }

		public AngleExceptions(string message, Exception inner) : base(message, inner) { }

		public AngleExceptions (string message,string id,string level, Exception inner) : base(message, inner)
		{
			Id = id;
			Level = level;
		}

		public string Id { get; }
		public string Level { get; }
		public IList<Exception> Inner { get; set; }
	}

}
