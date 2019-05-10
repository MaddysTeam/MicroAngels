using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

	public class AngelTokenRequest
	{
		public string Address { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Scopes { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Token { get; set; }
		public TokenRequestType RequestType { get; set; }
	}

	public enum TokenRequestType
	{
		client_credential=1,
		resource_password=2,
		revocation=3, // for ref tokens only
		refresh=4 
	}

}
