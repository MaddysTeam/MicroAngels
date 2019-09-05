using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;
using MicroAngels.Core;
using System.Linq;
using MicroAngels.Core.Plugins;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{

		public RoleController(IRoleService service)
		{
			_service = service;
		}

		[HttpPost("info")]
		public async Task<IActionResult> Get([FromForm] Guid id)
		{
			var role= await _service.GetById(id);
			var viewModel = role.Map<SystemRole, RoleViewModel>();

			return new JsonResult(new {
				data = viewModel
			});
		}

		[HttpPost("list")]
		public async Task<List<SystemRole>> List(string username)
		{
			return await _service.GetByUserName(username);
		}

		[HttpPost("roles")]
		public IActionResult GetRoles([FromForm]int start, [FromForm]int length)
		{
			var page = new PageOptions(start, length);
			var searchResults = _service.Search(null, page);

			return new JsonResult(new
			{
				data = searchResults.Select(role => role.Map<SystemRole, RoleViewModel>()),
				recordsTotal = page.TotalCount,
				recordsFiltered = page.TotalCount,
			});

		}

		[HttpPost("userRoles")]
		public async Task<IActionResult> GetRoelsByUserId([FromForm]Guid userId)
		{
			var result = await _service.GetByUserId(userId);
			return new JsonResult(new
			{

				data = result.Select(role => new
				{
					id = role.RoleId,
					name = role.RoleName,
					isChecked = !role.UserId.IsEmpty() && role.UserId == userId
				}).ToList()
			});
		}

		[HttpPost("edit")]
		public async Task<IActionResult> Edit([FromForm]RoleViewModel roleViewModel)
		{
			var role = roleViewModel.Map<RoleViewModel, SystemRole>();
			role.SystemId = Keys.System.DefaultSystemId;
			var isSuccess = await _service.Edit(role);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("BindAssets")]
		public async Task<IActionResult> BindResource([FromForm] BindAssetsViewModel roleAssets)
		{
			var isSuccess= await _service.BindResource(
				new RoleAssets
				{
					Id = Guid.NewGuid(),
					AssetId = roleAssets.AssetsId,
					RoleId = roleAssets.RoleId
				});

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}


		private readonly IRoleService _service;

	}

}
