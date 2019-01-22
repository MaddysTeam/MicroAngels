using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity
{
    /// <summary>
    /// TODO: will read resources from db asap
    /// </summary>
    public class Config
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                    {
                        SubjectId = "1",
                        Username = "alice",
                        Password = "password",
                        Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"superadmin") }
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "bob",
                        Password = "password",
                        Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"admin") }
                    }
            };

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
                new ApiResource("MessageCenter", "MESSAGE API",new List<string>{JwtClaimTypes.Role})
                {
                   // ApiSecrets = {new Secret("secret".Sha256())},
                   // UserClaims = ,
                }
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
                    ClientId = "messageClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = accessTokenLifetime,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MessageCenter" },
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowAccessTokensViaBrowser=true
                },
            };
        }
    }
}