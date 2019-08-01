using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{

	public class RoleController:Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Edit(Guid? id)
		{
			return PartialView("_edit",id);
		}

	}

}
