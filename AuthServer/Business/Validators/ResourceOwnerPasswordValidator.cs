using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        public ResourceOwnerPasswordValidator( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // you can change logic here 
            // for example : this can get user info by name and password , role info as well 

            //var claims = new List<Claim>()
            //    {
            //      //new Claim("Role","admin"),
            //      new Claim("sub","abcde|12345")
            //    };

            context.Result =
                new GrantValidationResult(
                    context.UserName,
                    "custom",//OidcConstants.AuthenticationMethods.Password,
                    GetUserClaims());

           // error
           // context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
        }
        private readonly IConfiguration _configuration;

        private Claim[] GetUserClaims()
        {
            return new Claim[]
            {
            //new Claim("UserId", 1.ToString()),
            //new Claim(JwtClaimTypes.Address,"wjk"),
            //new Claim(JwtClaimTypes.GivenName, "jaycewu"),
            //new Claim(JwtClaimTypes.FamilyName, "yyy"),
            //new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
            new Claim(JwtClaimTypes.Role,"superadmin")
            };
        }

    }

}
