using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    public static class ObjectExtension
    {

        public static bool IsNull(this object o)
        {
            return o == null || o.Equals(null);
        }

        public static void EnsureNotNull<Error>(this object o, Func<Error> errorFunc) where Error : Exception
        {
            if (o == null && !IsNull(errorFunc))
                throw errorFunc();
        }

    }

}
