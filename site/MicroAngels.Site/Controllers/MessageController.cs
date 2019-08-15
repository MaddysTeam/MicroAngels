using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{

	public class MessageController : Controller
	{


		//Get   message/AnnouncementIndex

		public IActionResult AnnounceIndex()
		{
			return View();
		}

		//Get   message/AnnounceSend

		public IActionResult AnnounceSend()
		{
			return PartialView("_announceSend");
		}


		//Get   message/index

		public IActionResult TopicIndex()
		{
			return View();
		}

		//Get   message/topicEdit

		public IActionResult TopicEdit(Guid id)
		{
			return PartialView("_editTopic", id);
		}

	}

}