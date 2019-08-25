using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Controllers
{

	public class UserController : Controller
	{

		public UserController(IConfiguration configuration)
		{
			_conf = configuration;
		}

		//Get   user/index

		public IActionResult Index()
		{
			return View();
		}

		//Get   user/Edit

		public IActionResult Edit(Guid? id)
		{
			ViewBag.ApiDomain = _conf["apis:apiDomain"];

			return PartialView("_edit",id);
		}

		//Get   user/bindRoles

		public IActionResult BindRoles(Guid userId)
		{
			return PartialView("_roles",userId);
		}

		//Get user/profile

		public IActionResult Profile(Guid userId)
		{
			return View(userId);
		}

		private IConfiguration _conf;

	}

}