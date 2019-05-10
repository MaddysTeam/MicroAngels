using AccountService.Models;
using MicroAngels.IdentityServer.Clients;
using MicroAngels.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		// POST api/validateNumber

		[HttpPost("validateNumber")]
		public string GetValidateNumber()
		{
			return string.Empty;
		}

		// GET api/signin

		[HttpPost("signin")]
		public async Task<string> SignIn([FromBody]LoginModel models)
		{
			// implement sign in logic here

			// get token response
			var response=await ClientHelper.GetTokenResponse(null,TokenRequestType.client_credential);

			return response.Token;
		}

		[HttpPost("signout")]
		[Authorize]
		public async Task<string> SignOut()
		{
			// implement sign out logic here

			var response = await ClientHelper.GetTokenResponse(null,TokenRequestType.revocation);

			return string.Empty;
		}

		[Authorize]
		[HttpPost("accountInfo")]
		public async Task<Account> AccountInfo(long id)
		{
			return new Account();
		}

		[HttpPost("signup")]
		public string SignUp(string username, string password)
		{
			return string.Empty;
		}

	}

}
