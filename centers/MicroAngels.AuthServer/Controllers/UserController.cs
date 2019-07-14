using Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("edit")]
		public async Task<bool> Edit([FromBody] UserInfo userInfo)
		{
			return await _userService.Edit(userInfo);
		}

		private IUserService _userService;

	}

}
