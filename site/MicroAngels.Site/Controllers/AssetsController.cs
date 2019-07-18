using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	public class AssetsController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}


		public  IActionResult InterfaceIndex()
		{
			return View();
		}

	}

}