using Business;
using IdentityServer4.Validation;
using MicroAngels.Cache.Redis;
using MicroAngels.IdentityServer.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MicroAngels.Core;

namespace MicroAngels.AuthServer.Services
{

	public class UserClaimGrantService : IClaimsGrantService
	{

		public UserClaimGrantService(IRoleService roleService, IUserService userService)
		{
			_roleService = roleService;
			_userService = userService;
			//_cache = cache;
		}

		public async Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context)
		{
			var claims = new List<Claim>();
			// get user id as client Id and put into claims 
			var users = await _userService.Search(u => u.UserName == context.UserName, null, null);
			if (!users.IsNull() && users.Count() > 0)
			{
				var userid = users.First().UserId.ToString();
				claims.Add(new Claim("userId", userid ));
			}
			else
			{
				return null;
			}

			// get user roles by user name
			var roles = await _roleService.GetByUserName(context.UserName);
			foreach (var role in roles)
			{
				claims.Add(new Claim(role.RoleName, "role"));
			}

			return claims.ToArray();
		}

		private readonly IRedisCache _cache;
		private readonly IRoleService _roleService;
		private readonly IUserService _userService;
	}

}
