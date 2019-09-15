using Business;
using Controllers;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
	//[ServiceFilter(typeof(LoggerAttribute))]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : BaseController
	{

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
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
		public async Task<IActionResult> SignIn([FromForm]LoginViewModel model)
		{
			//decrypt  password
			var response = await _accountService.SignIn(model);

			return new JsonResult(new
			{
				isSuccess = !response.IsError,
				token = response.Token,
				refreshToken = response.RefreshToken
			});
		}

		[HttpPost("signout")]
		public async Task<IActionResult> SignOut([FromForm]string token)
		{
			var userId = User.GetUserId();
			var response = await _accountService.SignOut(new SignoutViewModel { AccessToken = token, UserId = userId });

			return new JsonResult(new
			{
				isSuccess = true,
				message = "操作成功"
			});
		}

		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromForm] SignupViewModel signupModel)
		{
			var account = Mapper.Map<SignupViewModel, Account>(signupModel);
			var isSuccess = await _accountService.SignUp(account);

			if (isSuccess)
			{
				await _accountService.SendAddUserMessage(account);
			}

			return new JsonResult(new
			{
				isSuccess,
				message = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("refresh")]
		public async Task<AngelTokenResponse> Refresh([FromBody] RefreshTokenModel refreshTokenModel)
		{
			AngelTokenResponse response = await _accountService.Refresh(refreshTokenModel);

			return response;
		}

		[HttpPost("changePassword")]
		public async Task<bool> ChangePassword([FromForm] ChangePasswordViewModel model)
		{
			if (model.IsValidate)
			{
				return await _accountService.ChangePassword(model);
			}

			return false;
		}


		//[HttpPost("refresh")]
		//public async Task<bool> FogetPassword([FromBody] ChangePasswordViewModel model)
		//{
		//	return await _accountService.ChangePassword(model);
		//}


		private readonly IAccountService _accountService;

	}

}
