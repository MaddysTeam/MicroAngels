using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

		[HttpPost("users")]
		public IActionResult GetUsers([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var searchResults = _userService.Search(null, length, start, out totalCount);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x => new
					{
						username = x.UserName,
						realname = x.RealName,
						phone = x.Phone,
						email = x.Email,
						name5 = ""
					}),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,

				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}


		[HttpPost("info")]
		public IActionResult GetInfo([FromForm]string userName)
		{
			var requset = Request;
			var user = _userService.GetByName(userName);
			return new JsonResult(new
			{
				data = new { userId = user.UserId, username = user.UserName }
			});
		}


		[HttpPost("bindRole")]
		public IActionResult BindRole(Guid userId, Guid roleId)
		{
			var result = _userService.BindRole(new UserRole { Id = Guid.NewGuid(), RoleId = userId, UserId = userId });

			return new JsonResult(new {
				isSuccess = result,
				msg = ""
			});
		}

		private IUserService _userService;

	}

}
