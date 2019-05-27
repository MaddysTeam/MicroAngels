using MicroAngles.ORM.Suger;
using SqlSugar;

namespace Business
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		//public SimpleClient<Topic> TopicsDb { get { return new SimpleClient<Topic>(DB); } }

	}

}
