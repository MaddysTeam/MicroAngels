using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MicroAngels.Core
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

        public static T ToObject<T>(this string json)
        {
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

	}

}
