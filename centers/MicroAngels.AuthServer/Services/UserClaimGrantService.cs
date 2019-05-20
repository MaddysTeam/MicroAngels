using IdentityServer4.Validation;
using MicroAngels.IdentityServer.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class UserClaimGrantService : IClaimsGrantService
	{

		public async Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context)
		{
			return new Claim[] { new Claim("username", context.UserName) };

			//Claim(JwtClaimTypes.Role,"wjk"),
		}

	}

}
