using Business.Helpers;
using DotNetCore.CAP;
using MicroAngels.Bus.CAP;
using MicroAngels.Cache.Redis;
using MicroAngels.Core;
using MicroAngels.IdentityServer.Clients;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Business
{

	public class AccountService : MySqlDbContext, IAccountService
	{

		public AccountService(ILogger logger, ICAPPublisher publisher, IConfiguration configuraton, IRedisCache cache)
		{
			_logger = logger;
			_configuation = configuraton;
			_cache = cache;
			_publisher = publisher;
		}

		public async Task<AngelTokenResponse> Refresh(RefreshTokenModel model)
		{
			return await RefreshTokenRequest(model.AccessToken, model.RefreshToken);
		}

		public async Task<AngelTokenResponse> SignIn(LoginViewModel model)
		{
			if (!Validate(model.UserName, model.Password))
			{
				return new AngelTokenResponse { IsError = true, ErrorMessage = AlterKeys.Error.SIGNIN_FAILURE };
			}

			return await SignInTokenRequest(TokenRequestType.resource_password, model.UserName, model.Password);
		}

		public async Task<AngelTokenResponse> SignOut(SignoutViewModel model)
		{
			if (_cache.ExistKey(model.UserId))
			{
				_cache.Remove(model.UserId);
			}

			return await SignOutTokenRequest(model.AccessToken);
		}

		public async Task<bool> SignUp(Account model)
		{
			// implement sign up logic here

			var result = false;
			if (model.IsValidate)
			{
				var exitAccount = AccountDb.AsQueryable().FirstAsync(a => a.Name == model.Name);
				if (!exitAccount.IsNull())
				{
					result = false;
				}
				else
				{
					result = await AccountDb.AsInsertable(model).ExecuteCommandAsync() > 0;
				}
			}

			return result;
		}

		public async Task SendAddUserMessage(Account account)
		{
			await _publisher.PublishAsync(new AddUserMessage
			{
				Topic = AppKeys.AddUser,
				UserName = account.Name,
				Email = account.Email,
				Phone = account.Phone,
				Body = AppKeys.AddUser,
			});
		}

		public bool Validate(string name, string password)
		{
			// implement validate logic here

			return true;

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

		private async Task<AngelTokenResponse> RefreshTokenRequest(string token, string refreshToken)
		{
			var request = CreateBasiceRequest();
			request.Token = token;
			request.RefreshToken = refreshToken;
			var response = await ClientHelper.GetTokenResponse(request, TokenRequestType.refresh);

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
				GrantType = _configuation["IdentityService:Client:GrantType"]
			};
		}

		public Task<bool> SignUp(ChangePasswordViewModel model)
		{
			throw new System.NotImplementedException();
		}

		public async Task<bool> ChangePassword(ChangePasswordViewModel model)
		{
			var exitAccount = await AccountDb.AsQueryable().SingleAsync(a => a.Id == model.UserId.ToGuid() && a.Password == model.OldPassword.ToMD5());
			if (!exitAccount.IsNull())
			{
				exitAccount.ChangePassword(model.NewPassword.ToMD5());
				return AccountDb.Update(exitAccount);
			}

			return false;
		}


		[CapSubscribe(AppKeys.AddAccount,Group =AppKeys.AddAccount)]
		public async Task<bool> ReceiveAddAccountMessage(string message)
		{
			AddAccountMessage msg = JsonConvert.DeserializeObject<AddAccountMessage>(message);
			if (!msg.IsNull())
			{
				var account = new Account();
				return await SignUp(account);
			}

			return false;
		}

		private readonly ILogger _logger;
		private readonly IConfiguration _configuation;
		private readonly IRedisCache _cache;
		private readonly ICAPPublisher _publisher;
	}

}
