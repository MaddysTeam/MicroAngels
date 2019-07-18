using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Authorization;
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

		[HttpPost("urls")]
		public async Task<string[]> GetUrls([FromBody]string[] roles)
		{
			var interfaces = await _service.GetInterfaceByRoleNames(roles);

			return !interfaces.IsNull() && interfaces.Count() > 0 ? interfaces.Select(x => x.Url).ToArray() : null;
		}

		[HttpPost("allUrls")]
		public IActionResult GetAllUrls([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var interfacesResults = _service.GetInterface(null, length, start, out totalCount);

			if (!interfacesResults.IsNull() && interfacesResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = interfacesResults.Select(x => new
					{
						title = x.Title,
						url = x.Url,
						method = x.Method,
						param = x.Parmas,
						IsAnonymous = x.IsAllowAnonymous
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

		[HttpPost("menus")]
		public async Task<List<Menu>> GetMenus([FromBody]Guid userId)
		{
			var menus = await _service.GetMenusByUserId(userId);

			return menus.ToList();
		}


		private readonly IAssetsService _service;

	}

}
