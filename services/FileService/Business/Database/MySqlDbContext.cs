using MicroAngels.ORM.Suger;
using SqlSugar;
using System;

namespace FileService.Business
{

	public partial class MySqlDbContext
	{

		public SimpleClient<Files> db { get { return new SimpleClient<Files>(MySqlContext.Current.DB); } }

		public static Type[] TableTypes => new Type[] { typeof(Files) };
		
	}

}
