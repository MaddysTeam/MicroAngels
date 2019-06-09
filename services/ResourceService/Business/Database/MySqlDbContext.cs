using MicroAngels.ORM.Suger;
using SqlSugar;
using System;

namespace ResourceService.Business
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<Resource> ResourceDb { get { return new SimpleClient<Resource>(DB); } }

		public SimpleClient<ResourceFavorite> FavoriteDB { get { return new SimpleClient<ResourceFavorite>(DB); } }

		public SimpleClient<ResourceDownload> DownloadDB { get { return new SimpleClient<ResourceDownload>(DB); } }

		public static Type[] TableTypes = new Type[] { typeof(Resource), typeof(ResourceDownload),typeof(ResourceFavorite) };

	}

}
