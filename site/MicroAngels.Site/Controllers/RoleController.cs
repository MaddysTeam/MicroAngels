using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class RoleController:Controller
	{

		public IActionResult Index()
		{
			return View();
		}

	}

}
