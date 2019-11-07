using Microsoft.AspNetCore.Mvc;

namespace OcelotGateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HealthController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() =>
			Ok("Ocelot gateway status is ok");
	}
}
