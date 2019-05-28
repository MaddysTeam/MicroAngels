using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Plugins
{

	public static class Validator
	{

		private static Dictionary<object, List<ValidateResult>> _results = new Dictionary<object, List<ValidateResult>>();

		public static T Start<T>(this T t)
		{
			if (!_results.ContainsKey(t))
				_results.Add(t, new List<ValidateResult>());

			return t;
		}

		public static double Same(this double o, double t)
		{
			return o;
		}

		public static double GreaterThan(this double o, double t)
		{
			return o;
		}

		public static int Same(this int o, int t)
		{
			return o;
		}

		public static int GreaterThan(this int o, int t)
		{
			return o;
		}


		public static string Same(this string o, string t)
		{
			return o;
		}

		public static string NotEmpty(this string o)
		{
			return o;
		}

		public static string Length(this string o, int min, int max)
		{
			return o;
		}

		public static T And<S, T>(this S o, T other)
		{
			return other;
		}

		public static Dictionary<object, List<ValidateResult>> Execute(this object o)
		{
			return _results;
		}

	}

}
