using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Polly.Test
{

	public class PollyAopTarget
	{

		[Polly(
			EnableCircuitBroken =true,
			AllowedCountBeforeCirecuit =1)]
		public void RollBack()
		{
			throw new MicroAngels.Core.AngleExceptions("invoke polly", null);
		}

	}

}
