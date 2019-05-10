using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

	public class IdentityAuthenticationOptions
	{
		public string Scheme { get; set; }
		public string Authority { get; set; }
		public bool RequireHttps { get; set; }
		public string ApiName { get; set; }
		public string ApiSecret { get; set; }
	}

}
