using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    public static class StringExtension
    {

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static void EnsureNotNull<Error>(this string str, Func<Error> errorFunc) where Error : Exception
        {
            if (IsNullOrEmpty(str) && !errorFunc.IsNull())
                throw errorFunc();
        }

    }

}
