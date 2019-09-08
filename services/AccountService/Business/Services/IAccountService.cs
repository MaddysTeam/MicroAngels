﻿using Business;
using MicroAngels.IdentityServer.Models;
using System.Threading.Tasks;

namespace Business
{

	public interface IAccountService
	{
		Task<AngelTokenResponse> SignIn(LoginViewModel model);
		Task<bool> SignUp(Account model);
		Task<bool> SignUp(ChangePasswordViewModel model);
		Task<AngelTokenResponse> SignOut(string accessToken);
		Task<AngelTokenResponse> Refresh(RefreshTokenModel model);
		Task<bool> ChangePassword(ChangePasswordViewModel model);
		bool Validate(string name, string password);
	}

}
