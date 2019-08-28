using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
	[Produces("application/json")]
	[Route("api/Health")]
	[ApiController]
	public class HealthController : ControllerBase
	{

		[HttpGet]
		public IActionResult Get() => Ok("ok");

	}
}
