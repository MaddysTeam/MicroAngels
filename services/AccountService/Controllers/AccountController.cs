using AccountService.Models;
using MicroAngels.Cache.Redis;
using MicroAngels.Core.Plugins;
using MicroAngels.IdentityServer.Clients;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
	//[ServiceFilter(typeof(LoggerAttribute))]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		public AccountController(ILogger logger, IConfiguration configuraton)//IRedisCache cache)
		{
			_logger = logger;
			_configuation = configuraton;
			//_cache = cache;
		}

		// POST api/validateNumber

		[HttpPost("validateNumber")]
		public string GetValidateNumber()
		{
			return string.Empty;
		}

		// GET: api/privatekey

		[HttpPost("getPublicKey")]
		public string GetPublicKey()
		{
			var keys = RSACryptor.CreateKeys(2048, KeyFormat.XML);
			//where store private key? put into redis ?

			return keys[1];
		}

		// GET api/signin

		[HttpPost("signin")]
		public async Task<string> SignIn([FromBody]LoginModel model)
		{
			//decrypt  password

			var response = await SignInTokenRequest(TokenRequestType.resource_password, model.UserName, model.Password);

			return response.Token;
		}

		[Authorize]
		[HttpPost("signout")]
		public async Task<IActionResult> SignOut()
		{
			var response = await ClientHelper.GetTokenResponse(null, TokenRequestType.revocation);

			return Ok();
		}

		[Authorize]
		[HttpPost("info")]
		public Account Info(Guid id)
		{
			// get account info here
			var claims = Request.HttpContext.User.Claims;

			return new Account(Guid.NewGuid(), "Jimmy", DateTime.Now);
		}

		[HttpPost("signup")]
		public string SignUp(string username, string password)
		{
			// implement sign up logic here

			return string.Empty;
		}

		private async Task<AngelTokenResponse> SignInTokenRequest(TokenRequestType requestType, string username, string password)
		{
			var request = CreateBasiceRequest();
			if (requestType == TokenRequestType.resource_password)
			{
				request.UserName = username;
				request.Password = password;
			}

			var response = await ClientHelper.GetTokenResponse(request, requestType);

			return response;
		}

		private async Task<AngelTokenResponse> SignOutTokenRequest(string token)
		{
			var request = CreateBasiceRequest();
			request.Token = token;
			var response = await ClientHelper.GetTokenResponse(request, TokenRequestType.revocation);

			return response;
		}

		private AngelTokenRequest CreateBasiceRequest()
		{
			return new AngelTokenRequest
			{
				ClientId = _configuation["IdentityService:Client:Id"],
				ClientSecret = _configuation["IdentityService:Client:Secret"],
				Scopes = _configuation["IdentityService:Client:Scopes"],
				Address = _configuation["IdentityService:Client:Address"],
				GrantType = _configuation["IdentityService:Client:GrantType"],
				//RefreshToken = "9aa22e7229cfc841b89965cef6331d4205f861afd9dcd8cbbc32f29a172a6a6e",
			};
		}

		private readonly ILogger _logger;
		private readonly IConfiguration _configuation;
		private readonly IRedisCache _cache;

	}

}
