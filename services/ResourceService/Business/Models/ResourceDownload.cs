using SqlSugar;
using System;

namespace ResourceService.Business
{

	public class ResourceDownload
	{

		[SugarColumn(IsPrimaryKey = true)]
		public Guid OccurId { get; set; }

		[SugarColumn(IsNullable =false)]
		public Guid ResourceId { get; set; }

		[SugarColumn(IsNullable = false)]
		public Guid UserId { get; set; }

		public DateTime OccurTime { get; set; }

	}

}
