using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Authorization;
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
			var isSuccess = await _userService.Edit(user.Map<UserViewModel,UserInfo>());

			return new JsonResult(new
			{
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
					data = searchResults.Select(x => x.Map<UserInfo, UserViewModel>()),
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

		[HttpPost("briefInfo")]
		public async Task<IActionResult> GetInfo()
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var user = await _userService.GetById(userId.ToGuid());
			var viewMode = user.Map<UserInfo, UserViewModel>();

			return new JsonResult(new
			{
				data = viewMode
			});
		}

		[HttpPost("info")]
		public async Task<IActionResult> GetInfo([FromForm] Guid? id)
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			Guid uid = userId.IsNullOrEmpty() ? id.Value : userId.ToGuid();
			var user = await _userService.GetById(uid);

			return new JsonResult(new
			{
				data = user.Map<UserInfo, UserViewModel>()
			});
		}

		[HttpPost("bindRole")]
		public async Task<IActionResult> BindRole([FromForm] BindRoleViewModel viewModel)
		{
			var isSuccess = false;
			if (!viewModel.IsNull() && !viewModel.UserId.IsEmpty() && !string.IsNullOrEmpty(viewModel.RoleIds))
			{
				string[] roleIds = viewModel.RoleIds.Split(',');
				foreach (var roleId in roleIds)
				{
					isSuccess = await _userService.BindRole(new UserRole { RoleId = roleId.ToGuid(), UserId = viewModel.UserId });
				}

				return new JsonResult(new
				{
					isSuccess,
					msg = isSuccess ? "操作成功" : "操作失败"
				});
			}
			else
			{
				return new JsonResult(new
				{
					isSuccess,
					msg = "操作失败"
				});
			}

			//var result = _userService.BindRole(new UserRole { Id = Guid.NewGuid(), RoleId = userId, UserId = userId });

		}

		private IUserService _userService;

	}

}
