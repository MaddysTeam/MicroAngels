using IdentityServer4.AccessTokenValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Services
{

    public class ServiceAuthenticationOptions
    {
       public static string MessageApiAuthenticationKey = "MessageServiceKey";
       public static Action<IdentityServerAuthenticationOptions> MessageApiClient = option =>
        {
            option.Authority = "http://192.168.1.3:2012";
            option.ApiName = "MessageCenter";
            option.RequireHttpsMetadata = false;// Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
            option.SupportedTokens = SupportedTokens.Both;
            option.ApiSecret = "secret";// Configuration["IdentityService:ApiSecrets:clientservice"];
        };

    }

}
