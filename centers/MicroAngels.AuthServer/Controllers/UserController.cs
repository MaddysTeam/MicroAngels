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
		public async Task<IActionResult> Edit([FromForm]UserViewModel user)
		{
			var isSuccess= await _userService.Edit(new UserInfo { UserName=user.UserName, RealName=user.RealName, Email=user.RealName, Phone=user.Phone});

			return new JsonResult(new {
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
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
						id=x.UserId,
						username = x.UserName,
						realname = x.RealName,
						phone = x.Phone,
						email = x.Email
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
			var user = _userService.GetByName(userName);
			return new JsonResult(new
			{
				data = new { userId = user.UserId, username = user.UserName }
			});
		}

		[HttpPost("detail")]
		public IActionResult GetInfo([FromForm] Guid userId)
		{
			throw  new NotImplementedException();
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
