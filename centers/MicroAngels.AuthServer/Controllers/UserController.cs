using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("edit")]
		public async Task<bool> Edit([FromBody] UserInfo userInfo)
		{
			return await _userService.Edit(userInfo);
		}


		[HttpPost("users")]
		public IActionResult GetUsers([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var searchResults = _userService.Search(null, length, start, out totalCount);
			if(!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x=>new {
						username=x.UserName,
						realname=x.RealName,
						phone=x.Phone,
						email=x.Email,
						name5=""
					}),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,

				});
			}

			return new JsonResult(new {
				data=new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}

		//[HttpPost("users")]
		//public IActionResult GetUsers([FromForm]int start, [FromForm]int length)
		//{
		//	var forms = Request.Form;
		//	string[] userIdsArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" };
		//	var data = userIdsArray
		//		.Skip(start)
		//		.Take(length)
		//		.Select(x => new { name1 = "111", name2 = "2222", name3 = "3333", name4 = "4444", name5 = "5555" }).ToList();

		//	return new JsonResult(new
		//	{
		//		data = data,
		//		//draw = 2,
		//		recordsTotal = 11,
		//		recordsFiltered = 11,
		//	});
		//}

		private IUserService _userService;

	}

}
