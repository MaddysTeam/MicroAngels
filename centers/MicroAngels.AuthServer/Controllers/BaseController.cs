using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class BaseController : ControllerBase
	{

		public UserInfo GetUserInfo() => User.Get<UserInfo>();

		protected TokenViewModel CurrentToken => new TokenViewModel
		{
			accessToken = Request.Headers[CoreKeys.AccessToken].ToString(),
			refreshToken = Request.Headers[CoreKeys.RefreshToken].ToString()
		};

	}
}
