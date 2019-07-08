using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using System.Linq;
using MicroAngels.Core;

namespace MicroAngels.AuthServer.Test
{
	public class UserServiceTest : BaseTest
	{



		public UserServiceTest() : base()
		{
			_userService = Server.Host.Services.GetService<IUserService>();
		}

		[Fact]
		public async void InsertTest()
		{
			var user = AuthServerTestKeys.UserWithEmpty;
			var result = await _userService.Edit(user);
			Assert.False(result);

			user = AuthServerTestKeys.UserWIhtEmptyName;
			result = await _userService.Edit(user);
			Assert.False(result);

			user = AuthServerTestKeys.UserWIthNotLegalEmail;
			result = await _userService.Edit(user);
			Assert.False(result);

			user = AuthServerTestKeys.UserWIthNotLegalPhone;
			result = await _userService.Edit(user);
			Assert.False(result);

			user = AuthServerTestKeys.CorrectUser;
			result = await _userService.Edit(user);
			Assert.True(result);

			//var currentUsers = await _userService.Search(u => u.UserName == user.UserName,null,null);
			//var currentUser = currentUsers.IsNull() ? null:currentUsers.FirstOrDefault();
			//Assert.True(currentUser.UserName==user.UserName);
			//Assert.False(currentUser.UserId.IsEmpty());
		}



		[Fact]
		public async void ModifyTest()
		{

		}

		[Fact]
		public async void BindRoleTest()
		{
			var uesrRole = AuthServerTestKeys.UserRoleWithEmpty;
		}


		private IUserService _userService;

	}
}
