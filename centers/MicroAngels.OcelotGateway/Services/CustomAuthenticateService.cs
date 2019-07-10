using MicroAngels.Gateway.Ocelot;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class CustomAuthenticateService : ICustomAuthenticateService
	{

		public CustomAuthenticateService()
		{

		}

		public Task<bool> ValidateAuthenticate(HttpContext context, string path)
		{
			//TODO: 白名单和判断角色是否有接口权限
			var roleClaims = context.User.Claims.Where(c => c.Value == "role");
			if (roleClaims.Count() > 0)
			{
				var roles = roleClaims.Select(x => x.Type).ToArray();

				//var permissions = await _permissions.GetUrls(roles);
				//var hasPermission = permissions?.FirstOrDefault(url => url == requireUrl).IsNullOrEmpty();
				//if (hasPermission.IsNull() || hasPermission.Value)
				//{
				//	return false
				//}
			}


			return Task.FromResult(true);
		}

	}
}
