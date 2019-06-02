using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core
{

    public static class GuidExtensions
	{

        public static bool IsEmpty(this Guid o)
        {
			return o == Guid.Empty;
        }

	}

}
