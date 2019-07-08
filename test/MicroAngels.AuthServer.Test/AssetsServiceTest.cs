using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{

	public class AssetsServiceTest : BaseTest
	{

		//Interface currentterface = new Interface { Title = "my_interface", Url = "http://xxx.a.com",Method="GET" };

		public AssetsServiceTest() : base()
		{
			_service = Server.Host.Services.GetService<IAssetsService>();
		}

		[Fact]
		public async void InsertTest()
		{
			var assets = AuthServerTestKeys.AssetsWithNameEmpty;
			var result = await _service.Edit(assets);

			Assert.False(result);

			assets.AssetsName = AuthServerTestKeys.TempAssetName;
			result = await _service.Edit(assets);

			Assert.True(result);
		}

		[Fact]
		public async void ModifyTest()
		{
			var service = await _service.GetById(AuthServerTestKeys.AssetId);
			service.AssetsName = AuthServerTestKeys.TempAssetName;
			var result = await _service.Edit(service);
			Assert.True(result);

			service = await _service.GetById(AuthServerTestKeys.AssetId);
			Assert.True(service.AssetsName == AuthServerTestKeys.TempAssetName);
		}

		[Fact]
		public async void GetSingleTest()
		{
			var service = await _service.GetById(Guid.Empty);
			Assert.Null(service);

			service = await _service.GetById(AuthServerTestKeys.AssetId);
			Assert.NotNull(service);
			Assert.Equal(service.AssetsId, AuthServerTestKeys.AssetId);
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

			// fail due to url's pattern not matched
			iinterface.Title = "aaabbb";
			iinterface.Url = "aaabb";
			service = await _service.EditInterface(iinterface);
			Assert.False(service);

			service = await _service.EditInterface(AuthServerTestKeys.CorrectInterface);
			Assert.True(service);
		}

		[Fact]
		public void ModifyInterfaceTest()
		{

		}

		[Fact]
		public async void GetInterfaceByRoleNameTest()
		{
			//var assets = await _service.GetInterfaceByRoleNames(fakeRoleNames);
			//Assert.Null(assets);

			//assets = await _service.GetInterfaceByRoleNames(emptyRoleNames);
			//Assert.Null(assets);
		}


		private readonly IAssetsService _service;

	}

}
