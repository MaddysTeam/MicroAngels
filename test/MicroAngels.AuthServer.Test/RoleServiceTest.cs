using Business;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class RoleServiceTest : BaseTest
	{

		public RoleServiceTest()
		{
			var server = new TestServer(
				WebHost.CreateDefaultBuilder().UseContentRoot(
						GetProjectPath("MicroAngels.sln", "", typeof(Startup).Assembly)
					).UseStartup<Startup>()
				);

			_roleService = server.Host.Services.GetService<IRoleService>();
		}

		[Fact]
		public async void EditRoleTest()
		{
			var role = new SystemRole {  RoleName="father",   };
			var result = await _roleService.Edit(role);
		}


		private IRoleService _roleService;
		private Guid SystemId = Guid.Empty;

	}
}
