using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AccountService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HealthController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() => Ok("Account service is ok");
	}
}
