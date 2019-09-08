using MicroAngels.ORM.Suger;
using SqlSugar;
using System;

namespace Business
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<Account> AccountDb => new SimpleClient<Account>(DB);

		public static Type[] TableTypes = new Type[] {
			typeof(Account)
		};

	}

}
