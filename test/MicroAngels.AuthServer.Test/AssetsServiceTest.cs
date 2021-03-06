﻿using Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MicroAngels.AuthServer.Test
{

	public class AssetsServiceTest : BaseTest
	{

		public AssetsServiceTest() : base()
		{
			_service = Server.Host.Services.GetService<IAssetsService>();
		}

		[Fact]
		public async void InsertTest()
		{
			var result = await _service.Edit(AuthServerTestKeys.AssetsWithNameEmpty);
			Assert.False(result);
		}

		[Fact]
		public async void ModifyTest()
		{
			var service = await _service.GetById(AuthServerTestKeys.AssetId);
			service.AssetsName = AuthServerTestKeys.AssetName;
			var result = await _service.Edit(service);
			Assert.True(result);

			service = await _service.GetById(AuthServerTestKeys.AssetId);
			Assert.True(service.AssetsName == AuthServerTestKeys.AssetName);
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
			var service = await _service.EditInterface(AuthServerTestKeys.InterfaceWithNameEmpty);
			Assert.False(service);

			service = await _service.EditInterface(AuthServerTestKeys.InterfaceWithUrlEmpty);
			Assert.False(service);

			service = await _service.EditInterface(AuthServerTestKeys.CorrectInterface);
			Assert.True(service);
		}

		[Fact]
		public void InsertMenuTest()
		{

		}

		[Fact]
		public void ModifyInterfaceTest()
		{

		}

		[Fact]
		public async void GetInterfaceByRoleNameTest()
		{
			var assets = await _service.GetInterfaceByRoleNames(null);
			Assert.Null(assets);

			assets = await _service.GetInterfaceByRoleNames(new string[] { });
			Assert.Null(assets);
		}


		private readonly IAssetsService _service;

	}

}
