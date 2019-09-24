using Newtonsoft.Json;
using System;

namespace MicroAngels.Core
{

    public static class ObjectExtension
    {

        public static bool IsNull(this object o)
        {
            return o == null || o.Equals(null);
        }

        public static void EnsureNotNull<Error>(this object o, Func<Error> errorFunc) where Error : AngleExceptions
        {
            if (o == null && !IsNull(errorFunc))
                throw errorFunc();
        }

		public static string ToJson(this object o) 
		{
			return JsonConvert.SerializeObject(o);
		}

	}

}
