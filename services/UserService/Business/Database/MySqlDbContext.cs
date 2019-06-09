using MicroAngels.ORM.Suger;
using SqlSugar;
using System;

namespace UserService.Business
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<UserInfo> UserDb { get { return new SimpleClient<UserInfo>(DB); } }

		public SimpleClient<SystemRole> RoleDb { get { return new SimpleClient<SystemRole>(DB); } }

		public SimpleClient<SystemPrivilege> PrivilegeDb { get { return new SimpleClient<SystemPrivilege>(DB); } }

		public SimpleClient<UserRole> UserRoleDb { get { return new SimpleClient<UserRole>(DB); } }

		public static Type[] TableTypes = new Type[] { typeof(UserInfo), typeof(SystemRole),typeof(SystemPrivilege) };

	}

}
