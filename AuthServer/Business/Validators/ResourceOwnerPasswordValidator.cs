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

            var claims = new List<Claim>()
                {
                  new Claim("role","admin")
                };

            context.Result =
                new GrantValidationResult(
                    context.UserName,
                    OidcConstants.AuthenticationMethods.Password,
                    claims);

           // error
           // context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
        }
        private readonly IConfiguration _configuration;

    }

}
