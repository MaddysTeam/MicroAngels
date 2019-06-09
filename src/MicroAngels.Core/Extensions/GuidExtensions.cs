using System;

namespace MicroAngels.Core
{

	public static class GuidExtensions
	{

        public static bool IsEmpty(this Guid o)
        {
			return o == Guid.Empty;
        }

		public static void EnsureNotEmpty<Error>(this Guid o, Func<Error> errorFunc) where Error : Exception
		{
			if (o==Guid.Empty)
				throw errorFunc();
		}


	}

}
