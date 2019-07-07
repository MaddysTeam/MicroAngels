using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{

	public class AssetsServiceTest : BaseTest
	{

		public static Guid EnableStatus = Guid.Parse("db3c450e-defa-48c7-8b77-f0efd118f833");
		public static Guid DisableStatus = Guid.Parse("d3bc1ae8-7cf8-405b-9456-93b214d8a0d6");
		public static Guid InterfaceType = Guid.Parse("94348b14-58a8-4156-b3bf-a5940a706932");
		public static Guid AssetId = Guid.Parse("2d10128b-d3e0-4884-bf4d-9e6be36fad9c");
		public string assetsName1 = "asset_name1";
		public string assetsName2 = "asset_name2";
		string[] fakeRoleNames = { "unknow", "unknow2" };
		string[] emptyRoleNames = null;

		Interface currentterface = new Interface { Title = "my_interface", Url = "http://xxx.a.com",Method="GET" };

		public AssetsServiceTest() : base()
		{
			_service = Server.Host.Services.GetService<IAssetsService>();
		}

		[Fact]
		public async void InsertTest()
		{
			var assets = new Assets { AssetsName = "", AssetsStatus = EnableStatus, AssetsType = InterfaceType, SystemId = SystemServiceTest.SystemId };
			var result = await _service.Edit(assets);

			Assert.False(result);

			assets.AssetsName = assetsName1;
			result = await _service.Edit(assets);

			Assert.True(result);
		}

		[Fact]
		public async void ModifyTest()
		{
			var service = await _service.GetById(AssetId);
			service.AssetsName = assetsName2;
			var result = await _service.Edit(service);
			Assert.True(result);

			service = await _service.GetById(AssetId);
			Assert.True(service.AssetsName == assetsName2);
		}

		[Fact]
		public async void GetSingleTest()
		{
			var service = await _service.GetById(Guid.Empty);
			Assert.Null(service);

			service = await _service.GetById(AssetId);
			Assert.NotNull(service);
			Assert.Equal(service.AssetsId, AssetId);
		}


		[Fact]
		public async void InsertInterfaceTest()
		{
			// fail due to null value for title and url 
			var iinterface =new Business.Interface() {  };
			var service = await _service.EditInterface(iinterface);
			Assert.False(service);

			// fail due to null value for title and url 
			iinterface.Title = "";
			iinterface.Url = "";
			service = await _service.EditInterface(iinterface);
			Assert.False(service);

			// fail due to null value for url not matched pattern 
			iinterface.Title = "aaabbb";
			iinterface.Url = "aaabb";
			service = await _service.EditInterface(iinterface);
			Assert.False(service);

			service = await _service.EditInterface(currentterface);
			Assert.True(service);
		}


		[Fact]
		public void ModifyInterfaceTest()
		{

		}

		[Fact]
		public async void GetInterfaceByRoleNameTest()
		{
			var assets = await _service.GetInterfaceByRoleNames(fakeRoleNames);
			Assert.Null(assets);

			assets = await _service.GetInterfaceByRoleNames(emptyRoleNames);
			Assert.Null(assets);
		}


		private readonly IAssetsService _service;

	}

}
