using AccountService.Models;
using MicroAngels.IdentityServer.Clients;
using MicroAngels.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
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
		public async Task<string> SignIn()
		{
			// implement sign in logic here

			return string.Empty;
		}

		[Authorize]
		[HttpPost("signout")]
		public async Task<string> Refresh()
		{
			// implement refresh logic here

			var response = await ClientHelper.GetTokenResponse(new AngelTokenRequest
			{
				ClientId = "clientref",
				ClientSecret = "secreta",
				Scopes = " MessageCenter",
				Address = "http://192.168.1.8:2012/connect/token",
				RefreshToken = "9aa22e7229cfc841b89965cef6331d4205f861afd9dcd8cbbc32f29a172a6a6e",
				GrantType = "password",
			}, TokenRequestType.refresh);

			return response.Token;
		}

		[Authorize]
		[HttpPost("signout")]
		public async Task<string> SignOut()
		{
			// implement sign out logic here

			var response = await ClientHelper.GetTokenResponse(null,TokenRequestType.revocation);

			return string.Empty;
		}

		[Authorize]
		[HttpPost("info")]
		public async Task<Account> Info()
		{
			// get account info here

			return new Account(1,"Jimmy",DateTime.Now);
		}

		[HttpPost("signup")]
		public string SignUp(string username, string password)
		{
			// implement sign up logic here

			return string.Empty;
		}

	}

}
