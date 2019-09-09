using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	public class AccountController:Controller
	{

		public IActionResult Login()
		{
			return View();
		}


		public IActionResult ChangePassword(Guid userId)
		{
			return PartialView("_changePassword", userId);
		} 

	}

}
