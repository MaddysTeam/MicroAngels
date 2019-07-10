using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
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

		[HttpGet("getUrls")]
		public async Task<string[]> GetUrls(string roles)
		{
			var interfaces = await _service.GetInterfaceByRoleNames(roles.Split(","));
			return !interfaces.IsNull() && interfaces.Count() > 0 ? interfaces.Select(x => x.Url).ToArray() : null;
		}

		private readonly IAssetsService _service;

	}

}
