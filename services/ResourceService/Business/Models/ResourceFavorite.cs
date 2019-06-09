using SqlSugar;
using System;

namespace ResourceService.Business
{

	public class ResourceFavorite
	{

		//public ResourceFavorite(Guid occurId,Guid resourceId,Guid userId)
		//{
		//	OccurId = occurId;
		//	ResourceId = resourceId;
		//	UserId = userId;
		//}

		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public Guid OccurId { get;  set; }

		[SugarColumn(IsNullable =false,Length =50)]
		public Guid ResourceId { get;  set; }

		[SugarColumn(IsNullable = false)]
		public Guid UserId { get; private set; }

		public DateTime OccurTime { get;  set; }

	}

}
