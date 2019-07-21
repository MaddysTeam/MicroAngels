using Microsoft.AspNetCore.Mvc;

namespace Controllers
{

	public class UserController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		//Get   user/Edit

		public IActionResult Edit()
		{
			return PartialView("_edit");
		}


		[HttpPost]
		public IActionResult Edit(Temp user)
		{
			var req = Request;
			return new JsonResult(new {

			});
		}

		public class Temp
		{
			public string UserName { get; set; }
			public string RealName { get; set; }
		}

	}

}