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

		public IActionResult EditInterface()
		{
			return PartialView("_editMenu");
		}

		[HttpPost]
		public IActionResult GetAssets()
		{
			return new JsonResult(new
			{
				data = new TempViewModel
				{
					title = "【目录】系统设置",
					id = 1,
					parentId = 0,
					children = new List<TempViewModel>
					{
						 new TempViewModel
						 {
							title = "【目录】目录设置",
							id = 2,
							parentId = 1,
							isbind=true,
							children=new List<TempViewModel>()
						 }
					}
				}
			});
		}

		[HttpPost]
		public IActionResult Edit(List<TempViewModel> list)
		{
			return new JsonResult(new {

			});
		}


		[HttpPost]
		public IActionResult BindRoleAsset(string roleId,string assetId)
		{
			return new JsonResult(new
			{

			});
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