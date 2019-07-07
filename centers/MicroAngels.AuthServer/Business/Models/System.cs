using MicroAngels.Core.Domain;
using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{
	/// <summary>
	/// 系统
	/// </summary>
	public class System
	{

		[SugarColumn(IsPrimaryKey = true,Length =50)]
		public Guid SystemId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string SystemName { get; set; }

		[SugarColumn(IsNullable =true)]
		public string Code { get; set; }

		[SugarColumn(IsNullable = true)]
		public long Version { get; set; }

		[SugarColumn(IsNullable = true)]
		public string Description { get; set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid CreatorId { get; set; }

		public DateTime CreateTime { get; set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid ParentId { get; set; }

		public static List<ValidateResult> Validate(System system)
		{
			return
			system.NotNullOrEmpty(system.SystemName, "")
				 // .NotGuidEmpty(system.CreatorId, "")
				  .Validate();
		}

	}

}
