using Business;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

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
			var result = await _userService.Edit(AuthServerTestKeys.UserWithEmpty);
			Assert.False(result);

			result = await _userService.Edit(AuthServerTestKeys.UserWIhtEmptyName);
			Assert.False(result);

			result = await _userService.Edit(AuthServerTestKeys.UserWIthNotLegalEmail);
			Assert.False(result);

			result = await _userService.Edit(AuthServerTestKeys.UserWIthNotLegalPhone);
			Assert.False(result);

			result = await _userService.Edit(AuthServerTestKeys.CorrectUser);
			Assert.True(result);

			//var currentUsers = await _userService.Search(u => u.UserName == user.UserName,null,null);
			//var currentUser = currentUsers.IsNull() ? null:currentUsers.FirstOrDefault();
			//Assert.True(currentUser.UserName==user.UserName);
			//Assert.False(currentUser.UserId.IsEmpty());
		}

		[Fact]
		public  void ModifyTest()
		{

		}

		[Fact]
		public void GetUserTest()
		{

		}

		[Fact]
		public async void BindRoleTest()
		{
			//var result = await _userService.BindRole(AuthServerTestKeys.UserRoleWithEmpty);
			//Assert.False(result);

			//result = await _userService.BindRole(AuthServerTestKeys.CorrectUserRole);
			//Assert.False(result);

			//// duplicate user role
			//result = await _userService.BindRole(AuthServerTestKeys.CorrectUserRole);
			//Assert.False(result);
		}


		private IUserService _userService;

	}
}
