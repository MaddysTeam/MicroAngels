using IdentityModel;
using MicroAngels.IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class UserClaimGrantService : IClaimsGrantService
	{

		public Task<Claim[]> GetClaims()
		{
			return Task.FromResult(GetUserClaims());
		}


		private Claim[] GetUserClaims()
		{
			return new Claim[]
			{
			new Claim("UserId", 1.ToString()),
			new Claim("UserName","kissnana"),
		    //new Claim(JwtClaimTypes.Address,"wjk"),
		    //new Claim(JwtClaimTypes.GivenName, "jaycewu"),
		    //new Claim(JwtClaimTypes.FamilyName, "yyy"),
		    //new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
		    new Claim(JwtClaimTypes.Role,"superadmin")
			};
		}

	}

}
