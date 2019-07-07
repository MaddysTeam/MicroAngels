using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class RoleServiceTest : BaseTest
	{

		string roleName = "admin";

		public RoleServiceTest():base()
		{
			_roleService = Server.Host.Services.GetService<IRoleService>();
		}

		[Fact]
		public async void EditRoleTest()
		{
			var role = new SystemRole();
			var result = await _roleService.Edit(role);
			Assert.False(result);

			role.RoleName = roleName;
			role.SystemId = SystemId;
			result = await _roleService.Edit(role);
			Assert.True(result);

		}

		[Fact]
		public async void BindResourceTest()
		{
			var result= await _roleService.BindResource(null);
			Assert.False(result);
		}


		private IRoleService _roleService;
		private Guid SystemId = Guid.Empty;

	}
}
