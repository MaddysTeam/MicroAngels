using SqlSugar;
using System;

namespace ResourceService.Business
{

	public class CroFavorite
	{

		[SugarColumn(IsPrimaryKey = true)]
		public Guid OccurId { get; set; }

		public Guid ResourceId { get; set; }

		public Guid UserId { get; set; }

		public DateTime OccurTime { get; set; }

	}

}
