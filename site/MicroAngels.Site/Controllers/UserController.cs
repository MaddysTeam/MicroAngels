using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	public class UserController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GetUsers(int iDisplayStart, int iDisplayLength)
		{
			string[] userIdsArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" };
			var data = userIdsArray
				.Skip(iDisplayStart)
				.Take(iDisplayLength)
				.Select(x => new { name1 = "111", name2 = "2222", name3 = "3333", name4 = "4444", name5 = "5555" }).ToList();

			return Json(new
			{
				data = data,
				//draw = 2,
				recordsTotal = 11,
				recordsFiltered = 11,
			});
		}

	}

}