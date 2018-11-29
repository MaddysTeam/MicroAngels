using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Identity
{
    public class Config
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ProjectApi", "PROJECT API")
                {
                    ApiSecrets = {new Secret("secret".Sha256())},
                    UserClaims = new List<string>{"role"},
                },

                new ApiResource("TaskApi", "Task API")
                {
                    ApiSecrets = {new Secret("secret".Sha256())},
                    UserClaims = new List<string>{"role"},
                },

                new ApiResource("AccountApi", "Account API")
                {
                    ApiSecrets = {new Secret("secret".Sha256())},
                    UserClaims = new List<string>{"role"},
                },
            };
        }

        public IEnumerable<Client> GetClients()
        {
            var accessTokenLifetime = int.Parse(_configuration.GetConnectionString("AccessTokenLifetime"));

            // clients
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                        
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = accessTokenLifetime,
                    //RedirectUris={""},
                    //PostLogoutRedirectUris={ ""},
                    AllowedScopes = { "AccountApi","ProjectApi","TaskApi"},
                    AccessTokenType = AccessTokenType.Jwt
                },
            };
        }
    }
}