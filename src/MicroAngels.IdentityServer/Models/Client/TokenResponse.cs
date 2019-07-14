using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

	public class AngelTokenResponse
	{

		public string Token { get;  set; }

		public string RefreshToken { get; set; }

		public bool IsError { get; set; }

		public string StatusCode { get; set; }

		public string ErrorMessage { get; set; }

	}

}
