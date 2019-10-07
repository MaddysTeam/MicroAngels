using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

		public static bool IsSame(this string str, string target)
		{
			return string.Equals(str, target, StringComparison.InvariantCultureIgnoreCase);
		}

		public static Guid ToGuid(this string str)
		{
			var output = Guid.Empty;
			Guid.TryParse(str, out output);

			return output;
		}

		public static long ToLong(this string str)
		{
			long result = 0;
			long.TryParse(str, out result);

			return result;
		}


		public static string ToMD5(this string str)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			string result = BitConverter.ToString(md5.ComputeHash(bytes));
			return result.Replace("-", "");
		}

		public static bool IsBase64(this string str)
		{
			if (string.IsNullOrEmpty(str)) return false;

			try
			{
				Convert.FromBase64String(str);
				return true;
			}
			catch
			{
				return false;
			}
		}

	}

}
