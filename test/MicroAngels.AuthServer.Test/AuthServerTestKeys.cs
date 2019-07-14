using Business;
using System;
using System.Linq.Expressions;

namespace MicroAngels.AuthServer.Test
{
	public static class AuthServerTestKeys
	{

		public static Guid SystemId = Guid.Parse("da7f1c57-e62e-4faf-8028-9b848269e437");
		public static Guid AssetId = Guid.Parse("2d10128b-d3e0-4884-bf4d-9e6be36fad9c");
		public static Guid InterfaceId = Guid.Parse("915e5911-6f71-47cb-b0ff-1e0d355e2115");
		public static Guid RoleId = Guid.Parse("c2c1c796-2c94-4c19-9bfd-51df88839c9b");
		public static Guid UserId = Guid.Parse("8e69f758-94ff-4199-b1f9-44410766877f");
		public static Guid UserRoleId = Guid.Parse("3c5daa71-621a-48c6-9adf-1df7b8c09917");
		public static Guid RoleAssetId = Guid.Parse("65267c9f-bce4-40a9-ba31-ec17210243a9");

		public static Guid AssetEnableStatus = Guid.Parse("db3c450e-defa-48c7-8b77-f0efd118f833");
		public static Guid AssetDisableStatus = Guid.Parse("d3bc1ae8-7cf8-405b-9456-93b214d8a0d6");
		public static Guid InterfaceType = Guid.Parse("94348b14-58a8-4156-b3bf-a5940a706932");

		public static string SignupUrl = "api/accountservice/signup";

		public static string[] FackRoleNames = { "unknow", "unknow2" };
		public static string AssetName = "asset";
		public static string RoleName = "admin";
		public static string UserName = "jimmy";

		public static Expression<Func<SystemRole, bool>> roleCondition = r => r.RoleName == RoleName;

		public static Business.System SystemWithEmptyName = new Business.System { SystemName = "", SystemId = SystemId };
		public static Business.System CorrectSystem = new Business.System { SystemName = "",  Code="v1", CreateTime=DateTime.Now };

		public static Assets AssetsWithNameEmpty = new Assets { AssetsName = "", AssetsStatus = AuthServerTestKeys.AssetEnableStatus, AssetsType = AuthServerTestKeys.InterfaceType };
		public static Assets AssetWithSystemEmpty = new Assets { AssetsName = "" ,SystemId=Guid.Empty};

		public static Interface InterfaceWithNameEmpty = new Interface { Title = "", Method = "GET", Url = "http://xxx.a.com" };
		public static Interface InterfaceWithUrlEmpty = new Interface { Title = "interface1", Method = "POST", Url = "" };
		public static Interface InterfaceWihtIllegalUrl = new Interface { Title = "interface1", Method = "POST", Url = "aabbcc" };
		public static Interface CorrectInterface = new Interface { Title = "signup", Url = SignupUrl, Method = "POST" ,IsAllowAnonymous=true};

		public static SystemRole RoleWithNameEmpty = new SystemRole { RoleName="" };
		public static SystemRole RoleWithSystemIdEmpty = new SystemRole { RoleName = RoleName, SystemId = Guid.Empty };
		public static SystemRole CorrectRole = new SystemRole { RoleName = RoleName, SystemId = SystemId };

		public static RoleAssets RoleAssetWithEmptyRoleId = new RoleAssets { AssetId=AssetId, RoleId=Guid.Empty };
		public static RoleAssets RoleAssetWithEmptyAssetId = new RoleAssets { AssetId = Guid.Empty, RoleId = RoleId };
		public static RoleAssets CorrectRoleAsset = new RoleAssets { AssetId = AssetId, RoleId = RoleId };

		public static UserInfo UserWithEmpty = new UserInfo();
		public static UserInfo UserWIhtEmptyName = new UserInfo { UserName = "", RealName = "JimmyPoor" , };
		public static UserInfo UserWIthNotLegalEmail = new UserInfo { UserName = UserName, Email = "111,111" };
		public static UserInfo UserWIthNotLegalPhone = new UserInfo { UserName = UserName, Phone="xxx33233"};
		public static UserInfo CorrectUser = new UserInfo { UserName = UserName, Gender = 1, Phone = "15618528215", Email = "kissnana3@163.com" };

		public static UserRole UserRoleWithEmpty = new UserRole();
		public static UserRole UserRoleWithRmptyUserId = new UserRole { UserId=Guid.Empty, RoleId=RoleId };
		public static UserRole CorrectUserRole = new UserRole { RoleId=RoleId, UserId=UserId };

	}
}
