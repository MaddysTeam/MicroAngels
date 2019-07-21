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

		[HttpPost("get")]
		public async Task<SystemRole> Get(Guid id)
		{
			return await _service.GetById(id);
		}


		[HttpPost("list")]
		public async Task<List<SystemRole>> List(string username)
		{
			return await _service.GetByUserName(username);
		}

		[HttpPost("roles")]
		public IActionResult GetRoles([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var searchResults =  _service.Search(null, length, start, out totalCount);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(role => new
					{
						id=role.RoleId,
						name=role.RoleName,
						description=role.Description
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


		private readonly IRoleService _service;
		
	}

}
