using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class RoleController:Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Edit()
		{
			return PartialView("_edit");
		}

	}

}
