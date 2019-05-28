using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Plugins
{

	public class ValidateResult
	{
		public ValidateResult(bool isSuccess,string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; }
		public int ErrorCount { get; }
		public string Message { get; }
	}

}
