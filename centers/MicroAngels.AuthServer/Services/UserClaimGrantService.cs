using IdentityServer4.Validation;
using MicroAngels.IdentityServer.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class UserClaimGrantService : IClaimsGrantService
	{

		public Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context)
		{
			var claims= new Claim[] { new Claim("username", context.UserName) };

			return Task.FromResult(claims);

			//Claim(JwtClaimTypes.Role,"wjk"),
		}

	}

}
