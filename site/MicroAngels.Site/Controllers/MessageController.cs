using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{

	public class MessageController : Controller
	{

		//Get   user/index

		public IActionResult Index()
		{
			return View();
		}


		public IActionResult TopicIndex()
		{
			return View();
		}


	}

}