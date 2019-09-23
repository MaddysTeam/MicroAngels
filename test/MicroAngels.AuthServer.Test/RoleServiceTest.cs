using Business;
using MicroAngels.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace MicroAngels.AuthServer.Test
{
	public class RoleServiceTest : BaseTest
	{

		public RoleServiceTest() : base()
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
			Assert.False(role.RoleId.IsNull());

			// duplicated role is not allowed
			role = AuthServerTestKeys.DuplicatedRole;
			result = await _roleService.Edit(role);
			Assert.False(result);
		}

		[Fact]
		public async void BindResourceTest()
		{
			var result = await _roleService.BindResource(null);
			Assert.False(result);

			result = await _roleService.BindResource(AuthServerTestKeys.RoleAssetWithEmptyRoleId);
			Assert.False(result);

			result = await _roleService.BindResource(AuthServerTestKeys.RoleAssetWithEmptyAssetId);
			Assert.False(result);

			result = await _roleService.BindResource(AuthServerTestKeys.CorrectRoleAsset);
			Assert.True(result);

			// duplicate resource is not allowed
			result = await _roleService.BindResource(AuthServerTestKeys.CorrectRoleAsset);
			Assert.False(result);
		}

		[Fact]
		public async void GetRoleTest()
		{
			var result = await _roleService.GetByUserName(null);
			Assert.NotNull(result);
			Assert.True(result.Count == 0);

			result = await _roleService.GetByUserName(AuthServerTestKeys.UserName);
			Assert.NotNull(result);
			Assert.True(result.Count > 0);

			result = await _roleService.GetByUserId(AuthServerTestKeys.UserId);
			Assert.NotNull(result);

			result = await _roleService.GetByUserIds(new System.Guid[] { AuthServerTestKeys.UserId, AuthServerTestKeys.UserId2 });
			Assert.NotNull(result);
			Assert.True(result.Count > 0);
		}

		[Fact]
		public  void SearchRoleTest()
		{
			var result = _roleService.Search(null, null);
			Assert.NotNull(result);
			Assert.True(result.Count() > 0);

			int pageSize = 10;
			int startIndex = 0;
			result = _roleService.Search(null, new PageOptions(startIndex, pageSize));
			Assert.NotNull(result);
			Assert.True(result.Count() == 10);
		}


		private IRoleService _roleService;

	}
}
