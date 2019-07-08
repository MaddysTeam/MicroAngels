using Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.AuthServer.Test
{
	public static class AuthServerTestKeys
	{

		public static Guid SystemId = Guid.Parse("da7f1c57-e62e-4faf-8028-9b848269e437");
		public static Guid AssetId = Guid.Parse("2d10128b-d3e0-4884-bf4d-9e6be36fad9c");
		public static Guid InterfaceId = Guid.Parse("");
		public static Guid RoleId = Guid.Parse("");
		public static Guid UserId = Guid.Parse("");

		public static Guid AssetEnableStatus = Guid.Parse("db3c450e-defa-48c7-8b77-f0efd118f833");
		public static Guid AssetDisableStatus = Guid.Parse("d3bc1ae8-7cf8-405b-9456-93b214d8a0d6");
		public static Guid InterfaceType = Guid.Parse("94348b14-58a8-4156-b3bf-a5940a706932");
		
		public static string[] FackRoleNames = { "unknow", "unknow2" };
		public static string TempAssetName = "";
		public static string RoleName = "admin";
		public static string UserName = "jimmy";

		public static Business.System SystemWithEmptyName = new Business.System { SystemName = "", SystemId = SystemId };
		public static Business.System CorrectSystem = new Business.System { SystemName = "",  Code="v1", CreateTime=DateTime.Now };

		public static Assets AssetsWithNameEmpty = new Assets { AssetsName = "", AssetsStatus = AuthServerTestKeys.AssetEnableStatus, AssetsType = AuthServerTestKeys.InterfaceType };
		public static Assets AssetWithSystemEmpty = new Assets { AssetsName = "" ,SystemId=Guid.Empty};
		public static Interface CorrectInterface = new Interface { Title = "my_interface", Url = "http://xxx.a.com", Method = "GET" };

		public static SystemRole RoleWithNameEmpty = new SystemRole { RoleName="" };
		public static SystemRole RoleWithSystemIdEmpty = new SystemRole { RoleName = RoleName, SystemId = Guid.Empty };
		public static SystemRole CorrectRole = new SystemRole { RoleName = RoleName, SystemId = SystemId };

		public static RoleAssets RoleAssetWithEmptyRoleId = new RoleAssets { AssetId=AssetId, RoleId=Guid.Empty };
		public static RoleAssets RoleAssetWithEmptyAssetId = new RoleAssets { AssetId = Guid.Empty, RoleId = RoleId };

		public static UserInfo UserWithEmpty = new UserInfo();
		public static UserInfo UserWIhtEmptyName = new UserInfo { UserName = "", RealName = "JimmyPoor" , };
		public static UserInfo UserWIthNotLegalEmail = new UserInfo { UserName = UserName, Email = "111,111" };
		public static UserInfo UserWIthNotLegalPhone = new UserInfo { UserName = UserName, Phone="xxx33233"};
		public static UserInfo CorrectUser = new UserInfo { UserName = UserName, Gender = 1, Phone = "15618528215", Email = "kissnana3@163.com" };

		public static UserRole UserRoleWithEmpty = new UserRole();
		public static UserRole UserRoleWithRmptyUserId = new UserRole { UserId=Guid.Empty, RoleId=RoleId };


	}
}
