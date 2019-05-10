using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MicroAngels.IdentityServer.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Validators
{

	public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        public ResourceOwnerPasswordValidator(IUserValidateService serivce, IClaimsGrantService grantService,IConfiguration configuration)
        {
			_service = serivce;
			_grantService = grantService;
			_configuration = configuration;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
			// you can change logic here 
			// for example : this can get user info by name and password , role info as well 

			if (await _service.ValidatePassword(context.UserName, context.Password))
			{
				var claims =await _grantService.GetClaims();
				context.Result =
					new GrantValidationResult(
						context.UserName,
						OidcConstants.AuthenticationMethods.Password,
						claims);
			}
			else
			{
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
			}
		}

        private readonly IConfiguration _configuration;
		private IUserValidateService _service;
		private IClaimsGrantService _grantService;

	}

}
