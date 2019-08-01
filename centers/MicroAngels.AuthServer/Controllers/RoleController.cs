using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;
using MicroAngels.Core;
using System.Linq;

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
			var result= await _service.GetById(id);

			return new JsonResult(new {
				data = new RoleViewModel { Id = result.RoleId, RoleName = result.RoleName, Description=result.Description	 }
			});
		}

		[HttpPost("list")]
		public async Task<List<SystemRole>> List(string username)
		{
			return await _service.GetByUserName(username);
		}

		[HttpPost("roles")]
		public IActionResult GetRoles([FromForm]int? start, [FromForm]int? length)
		{
			var totalCount = 0;
			var searchResults = _service.Search(null, length, start, out totalCount);

			return new JsonResult(new
			{
				data = searchResults.Select(role => new
				{
					id = role.RoleId,
					name = role.RoleName,
					description = role.Description
				}),
				recordsTotal = totalCount,
				recordsFiltered = totalCount,
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
		public async Task<IActionResult> Edit([FromForm]RoleViewModel role)
		{
			var isSuccess = await _service.Edit(
				new SystemRole
				{
					RoleId = role.Id,
					Description = role.Description,
					RoleName = role.RoleName,
					SystemId = Keys.System.DefaultSystemId
				});

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
