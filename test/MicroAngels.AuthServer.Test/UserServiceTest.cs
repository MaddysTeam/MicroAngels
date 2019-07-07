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
			var user = new UserInfo();
			var result = await _userService.Edit(user);
			Assert.False(result);

			user.UserName = "hello";
			result = await _userService.Edit(user);
			Assert.True(result);

			var currentUsers = await _userService.Search(u => u.UserName == user.UserName,null,null);
			var currentUser = currentUsers.IsNull() ? null:currentUsers.FirstOrDefault();
			Assert.True(currentUser.UserName==user.UserName);
			Assert.False(currentUser.UserId.IsEmpty());
		}



		[Fact]
		public async void ModifyTest()
		{

		}

		[Fact]
		public async void BindRoleTest()
		{

		}


		private IUserService _userService;
		private Guid SystemId = Guid.Empty;

	}
}
