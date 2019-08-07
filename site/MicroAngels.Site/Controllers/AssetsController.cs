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


		public IActionResult InterfaceIndex()
		{
			return View();
		}

		public IActionResult MenuIndex()
		{
			return View();
		}

		public IActionResult EditMenu(Guid? menuId)
		{
			return PartialView("_editMenu", menuId);
		}

		public IActionResult EditInterface(Guid? interfaceId)
		{
			return PartialView("_editInterface", interfaceId);
		}

	}

	public class TempViewModel
	{
		public string title { get; set; }
		public int id { get; set; }
		public int parentId { get; set; }
		public bool isbind { get; set; }
		public List<TempViewModel> children { get; set; }
	}

}