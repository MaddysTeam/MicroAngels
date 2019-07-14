using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AssetsController : ControllerBase
	{

		public AssetsController(IAssetsService service)
		{
			_service = service;
		}

		[HttpPost("users")]
		public async Task<string[]> GetUrls([FromBody]string[] roles)
		{
			var interfaces = await _service.GetInterfaceByRoleNames(roles);

			return !interfaces.IsNull() && interfaces.Count() > 0 ? interfaces.Select(x => x.Url).ToArray() : null;
		}

		[HttpPost("menus")]
		public async Task<List<Menu>> GetMenus([FromBody]Guid userId)
		{
			var menus = await _service.GetMenusByUserId(userId);

			return menus.ToList();
		}

		private readonly IAssetsService _service;

	}

}
