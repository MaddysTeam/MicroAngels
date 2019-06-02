using FileService.Business;
using MicroAngels.ORM.Suger;
using SqlSugar;

namespace FileService.Business
{

	public partial class MySqlDbContext
	{

		//public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<Files> db { get { return new SimpleClient<Files>(MySqlContext.Current.DB); } }

	}

}
