using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class RoleServiceTest : BaseTest
	{

		public RoleServiceTest():base()
		{
			_roleService = Server.Host.Services.GetService<IRoleService>();
		}

		[Fact]
		public async void EditRoleTest()
		{
			var role = AuthServerTestKeys.RoleWithNameEmpty;
			var result = await _roleService.Edit(role);
			Assert.False(result);

			role = AuthServerTestKeys.RoleWithSystemIdEmpty;
			result = await _roleService.Edit(role);
			Assert.False(result);

			role = AuthServerTestKeys.CorrectRole;
			result = await _roleService.Edit(role);
			Assert.True(result);
		}

		[Fact]
		public async void BindResourceTest()
		{
			var result= await _roleService.BindResource(null);
			Assert.False(result);

			result = await _roleService.BindResource(AuthServerTestKeys.RoleAssetWithEmptyRoleId);
			Assert.False(result);

			result = await _roleService.BindResource(AuthServerTestKeys.RoleAssetWithEmptyAssetId);
			Assert.False(result);
		}

		[Fact]
		public async void GetRoleByUserNameTset()
		{
			
		}


		private IRoleService _roleService;

	}
}
