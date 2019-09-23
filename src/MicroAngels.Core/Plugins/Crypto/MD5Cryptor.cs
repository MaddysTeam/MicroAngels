using System;
using System.IO;
using System.Security.Cryptography;

namespace MicroAngels.Core.Plugins
{

	public class MD5Cryptor
	{

		public static string Encrypt(Stream source)
		{

			using (var md5 = MD5.Create())
			{
				var result = md5.ComputeHash(source);
				var strResult = BitConverter.ToString(result);
				return strResult.Replace("-", "");
			}

		}

	}

}
