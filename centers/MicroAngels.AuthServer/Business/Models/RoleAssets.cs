using SqlSugar;
using System;

namespace Business
{

	public class RoleAssets
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid Id { get; set; }
		public Guid AssetId { get; set; }
		public Guid RoleId { get; set; }
	}

}
