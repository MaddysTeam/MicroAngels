using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class SystemServiceTest : BaseTest
	{

		public SystemServiceTest() : base()
		{
			_systemService = Server.Host.Services.GetService<ISystemService>();
		}


		[Fact]
		public async void InsertTest()
		{
			//insert instance failure due to name null
			var result = await _systemService.Edit(AuthServerTestKeys.SystemWithEmptyName);
			Assert.False(result);

			result = await _systemService.Edit(AuthServerTestKeys.CorrectSystem);
			Assert.True(result);
		}

		[Fact]
		public async void ModifyTest()
		{
			var system = new Business.System { SystemId = AuthServerTestKeys.SystemId, SystemName = "System2", CreateTime = DateTime.Now };
			var result = await _systemService.Edit(system);
			Assert.True(result);

			//edit instance failure due to name null
			system.SystemName = "";
			result = await _systemService.Edit(system);
			Assert.False(result);
		}

		[Fact]
		public async void SearchByIdTest()
		{
			var system = await _systemService.GetById(Guid.Empty);
			Assert.Null(system);

			system = await _systemService.GetById(AuthServerTestKeys.SystemId);
			Assert.NotNull(system);
		}


		private ISystemService _systemService;

	}
}
