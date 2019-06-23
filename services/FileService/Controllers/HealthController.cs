using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FileService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HealthController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() => Ok("File service is ok");
	}
}
