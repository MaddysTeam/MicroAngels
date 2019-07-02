using Business;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class SystemServiceTest : BaseTest
	{

		public SystemServiceTest()
		{
			var server = new TestServer(
				WebHost.CreateDefaultBuilder().UseContentRoot(
						GetProjectPath("MicroAngels.sln", "", typeof(Startup).Assembly)
					).UseStartup<Startup>()
				);

			_systemService = server.Host.Services.GetService<ISystemService>();
		}

		[Fact]
		public async void EditRoleTest()
		{
			var system = new Business.System { SystemName = "", CreatorId = Guid.Empty, Description = "" };
			var result = await _systemService.Edit(system);
		}


		private ISystemService _systemService;

	}
}
