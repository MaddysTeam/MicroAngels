using IdentityServer4.AccessTokenValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.OcelotGateway.Services
{

	public class ServiceAuthenticationOptions
	{
		public static string GlobalApiAuthenticationKey = "MessageServiceKey";
		//public static string AccountApiAuthenticationKey = "AccountServiceKey";

		public static Action<IdentityServerAuthenticationOptions> GlobalApiClient = option =>
		 {
			 option.Authority = "http://192.168.1.3:2012";
			 option.ApiName = "MessageCenter";
			 option.RequireHttpsMetadata = false;// Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
			 option.SupportedTokens = SupportedTokens.Both;
			 option.ApiSecret = "secreta";// Configuration["IdentityService:ApiSecrets:clientservice"];
		};

	}

}
