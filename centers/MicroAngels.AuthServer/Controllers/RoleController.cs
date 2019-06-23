using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;

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

		private readonly IRoleService _service;
		
	}

}
