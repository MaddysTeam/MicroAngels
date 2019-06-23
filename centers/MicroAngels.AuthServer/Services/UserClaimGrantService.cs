using Business;
using IdentityServer4.Validation;
using MicroAngels.Cache.Redis;
using MicroAngels.IdentityServer.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MicroAngels.AuthServer.Services
{

	public class UserClaimGrantService : IClaimsGrantService
	{

		public UserClaimGrantService(IRoleService roleService, IRedisCache cache)
		{
			_roleService = roleService;
			_cache = cache;
		}

		public async Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context)
		{
			// get user roles by user name
			var roles = await _roleService.GetByUserName(context.UserName);
			var claims = new List<Claim>();
			foreach (var role in roles)
			{
				claims.Add( new Claim(role.RoleName, "role"));
			}

			return claims.ToArray();
		}

		private readonly IRedisCache _cache;
		private readonly IRoleService _roleService;
	}

}
