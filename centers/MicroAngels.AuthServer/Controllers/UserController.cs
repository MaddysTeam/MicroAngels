using Business;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseController
	{

		public UserController(IUserService userService, ICAPPublisher publisher)
		{
			_userService = userService;
			_publisher = publisher;
		}

		[HttpPost("edit")]
		public async Task<IActionResult> Edit([FromForm]UserViewModel user)
		{
			//var isInsert = user.Id.IsEmpty();
			var userInfo = user.Map<UserViewModel, UserInfo>();
			var isSuccess = true; //await _userService.Edit(userInfo);

			if (true)
				await _userService.SendAddAccountMessage(userInfo);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("users")]
		public async Task<IActionResult> GetUsers([FromForm]int start, [FromForm]int length)
		{
			var userId = User.GetUserId();
			var serviceId = User.GetServiceId();
			var page = new PageOptions(start, length);
			var searchResults =await _userService.SearchWithFriends(new UserSearchOption { UserId=userId, ServiceId=serviceId, TopicId="" },page);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x => x.Map<UserInfo, UserViewModel>()),
					recordsTotal = page.TotalCount,
					recordsFiltered = page.TotalCount,
				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}

		[HttpPost("friends")]
		public async Task<IActionResult> GetFriends([FromForm]int start, [FromForm]int length)
		{
			var userId = User.GetUserId();
			var serviceId = User.GetServiceId();
			var page = new PageOptions(start, length);
			var searchResults = await _userService.SearchWithFriends(new UserSearchOption { UserId = userId, ServiceId = serviceId, TopicId = "" }, page);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				var onlyFriends = searchResults.ToList().FindAll(x => x.IsFriend);
				return new JsonResult(new
				{
					data = onlyFriends?.Select(x => x.Map<UserInfo, UserViewModel>()),
					recordsTotal = page.TotalCount,
					recordsFiltered = page.TotalCount,
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
				data = viewMode,
				tokens = CurrentToken,
			});
		}

		[HttpPost("info")]
		public async Task<IActionResult> GetInfo([FromForm] Guid id)
		{
			var user = await _userService.GetById(id);

			return new JsonResult(new
			{
				data = user.Map<UserInfo, UserViewModel>(),
			});
		}

		[HttpPost("bindRole")]
		public async Task<IActionResult> BindRole([FromForm] BindRoleViewModel viewModel)
		{
			var isSuccess = false;
			if (!viewModel.IsNull() && !viewModel.UserId.IsEmpty())
			{
				string[] roleIds = viewModel.RoleIds.IsNullOrEmpty() ? new string[] { } : viewModel.RoleIds.Split(',');
				isSuccess = await _userService.BindRoles(viewModel.UserId, roleIds);

				return new JsonResult(new
				{
					isSuccess,
					msg = isSuccess ? "操作成功" : "操作失败",
				});
			}
			else
			{
				return new JsonResult(new
				{
					isSuccess,
					msg = "操作失败",
				});
			}

			//var result = _userService.BindRole(new UserRole { Id = Guid.NewGuid(), RoleId = userId, UserId = userId });

		}

		private IUserService _userService;
		private ICAPPublisher _publisher;

	}

}
