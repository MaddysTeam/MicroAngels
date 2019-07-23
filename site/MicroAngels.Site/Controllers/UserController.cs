using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{

	public class UserController : Controller
	{

		//Get   user/index

		public IActionResult Index()
		{
			return View();
		}

		//Get   user/Edit

		public IActionResult Edit()
		{
			return PartialView("_edit");
		}

		//Get   user/bindRoles

		public IActionResult BindRoles(Guid userId)
		{
			return PartialView("_roles",userId);
		}

	}

}