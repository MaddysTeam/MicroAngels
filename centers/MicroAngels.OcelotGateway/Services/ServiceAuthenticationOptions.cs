using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.Configuration;
using System;

namespace MicroAngels.OcelotGateway.Services
{

	public class ServiceAuthenticationOptions
	{
		public ServiceAuthenticationOptions(IConfiguration configuration)
		{
			_conf = configuration;
		}

		public  string GlobalApiAuthenticationKey = "MessageServiceKey";
		//public static string AccountApiAuthenticationKey = "AccountServiceKey";

		public Action<IdentityServerAuthenticationOptions> GlobalApiClient = option =>
		{
			option.Authority = _conf["IdentityServices:Uri"];
			option.ApiName = _conf["IdentityServices:Audience"];
			option.RequireHttpsMetadata = Convert.ToBoolean(_conf["IdentityServices:UseHttps"]);
			option.SupportedTokens = SupportedTokens.Both;
			option.ApiSecret = _conf["IdentityServices:ApiSecret"];
		};

		 static IConfiguration _conf;

	}

}
