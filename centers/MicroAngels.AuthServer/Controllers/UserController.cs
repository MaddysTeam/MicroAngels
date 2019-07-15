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
		public async Task<List<UserInfo>> GetUsers(int? pageSize,int? pageIndex)
		{
			var results= await _userService.Search(null, pageSize, pageIndex);
			if(!results.IsNull() && results.Count() > 0)
			{
				return results.ToList();
			}

			return null;
		} 

		private IUserService _userService;

	}

}
