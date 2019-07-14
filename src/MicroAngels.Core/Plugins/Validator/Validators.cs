using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MicroAngels.Core.Plugins
{

	/// <summary>
	/// validator for entity
	/// </summary>
	public static class Validator
	{

		private static IDictionary<object, List<ValidateResult>> _results = new Dictionary<object, List<ValidateResult>>();

		public static T Same<T>(this T t, int s, int r, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(s != r, errorMessage);
		}

		public static T GreaterThan<T>(this T t, int s, int r, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(s < r, errorMessage);

		}

		public static T Same<T>(this T t, string s, string r, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(s != r, errorMessage);
		}

		public static T Length<T>(this T t, string s, int length, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(s.Length != length, errorMessage);
		}

		public static T NotNull<T>(this T t, object o, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(o != null, errorMessage);
		}

		public static T NotNullOrEmpty<T>(this T t, string o, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(!string.IsNullOrEmpty(o), errorMessage);
		}

		public static T NotGuidEmpty<T>(this T t, Guid o, string errorMessage)
		{
			t.Init();

			return t.WhenInvalid(o != Guid.Empty, errorMessage);
		}


		public static T RegexIsMatch<T>(this T t, string s, string patterns,string errorMessage)
		{
			t.Init();
			
			return t.WhenInvalid<T>(Regex.Match(s,patterns).Success,errorMessage);
		}

		public static T IsIn<T,V>(this T t,V[] array,V v) where V: IComparable
		{
			t.Init();

			foreach(var item in array)
			{
				if (item.CompareTo(v)==0)
				{
					return t.WhenInvalid(true,"");
				}
			}

			return t.WhenInvalid(false,"");
		}


		public static List<ValidateResult> Validate<T>(this T o)
		{
			var result = new List<ValidateResult>();
			if (_results.ContainsKey(o))
			{
				result = _results[o];
				_results.Remove(o);
			}

			return result;
		}


		private static T Init<T>(this T t)
		{
			if (!_results.ContainsKey(t))
				_results.Add(t, new List<ValidateResult>());

			return t;
		}

		private static T WhenInvalid<T>(this T t, bool isInvalid, string errorMessage)
		{
			if (!isInvalid && _results.ContainsKey(t))
			{
				_results[t].Add(new ValidateResult(false, errorMessage));
			}

			return t;
		}

	}

}
