using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	public class RoleAssets
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid Id { get; set; }
		public Guid AssetId { get; set; }
		public Guid RoleId { get; set; }

		public static List<ValidateResult> Validate(RoleAssets ra)
		{
			return
			ra.NotGuidEmpty(ra.AssetId, "")
			  .NotGuidEmpty(ra.RoleId, "")
			  .Validate();
		}
	}

}
