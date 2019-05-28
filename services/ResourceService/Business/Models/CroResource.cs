using SqlSugar;
using System;
using MicroAngels.Core.Plugins;

namespace ResourceService.Business
{

	public class CroResource
	{

		[SugarColumn(IsPrimaryKey = true)]
		public Guid CrosourceId { get; set; }

		public Guid FildId { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }

		public string Keywords { get; set; }

		public Guid MediumTypeId { get; set; }

		public int DownCount { get; set; }

		public int FavoriteCount { get; set; }

		public int CommentCount { get; set; }

		public int StarCount { get; set; }

		public static void Validate(CroResource r)
		{
			//var results =
			//r.Start()
			// .Title.Same("")
			// .And(r.DownCount).Same(10)
			// .Execute();
		}

	}

}
