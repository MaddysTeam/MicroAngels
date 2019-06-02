using MicroAngels.ORM.Suger;
using SqlSugar;

namespace ResourceService.Business
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<CroResource> ResourceDb { get { return new SimpleClient<CroResource>(DB); } }

		public SimpleClient<CroFavorite> FavoriteDB { get { return new SimpleClient<CroFavorite>(DB); } }

	}

}
