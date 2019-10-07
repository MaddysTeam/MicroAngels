using Business;
using IdentityServer4.Validation;
using MicroAngels.Core;
using MicroAngels.IdentityServer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{
	/// <summary>
	/// Grant serivce been invoke before generating access tokens in identity server
	/// </summary>
	public class UserClaimGrantService : IClaimsGrantService
	{

		public UserClaimGrantService(IRoleService roleService, IUserService userService)
		{
			_roleService = roleService;
			_userService = userService;
		}

		public async Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context)
		{
			var claims = new List<Claim>();

			// get user id as client Id and put into claims 
			var users = await _userService.Search(u => u.UserName == context.UserName, null);
			if (!users.IsNull() && users.Count() > 0)
			{
				var user = users.First();
				var userid = user.UserId.ToString();
				var realName = user.RealName;
				claims.Add(new Claim(CoreKeys.USER_ID, userid));
				claims.Add(new Claim(CoreKeys.USER, user.ToJson()));
			}

			// get user roles by user name
			var roles = await _roleService.GetByUserName(context.UserName);
			foreach (var role in roles)
			{
				claims.Add(new Claim(role.RoleName, CoreKeys.ROLE));
			}

			// set serviceId(systemid) to claim 
			claims.Add(new Claim(CoreKeys.SYSTEM_ID, Keys.System.DefaultSystemId.ToString()));

			return claims.ToArray();
		}

		private readonly IRoleService _roleService;
		private readonly IUserService _userService;

	}

}
