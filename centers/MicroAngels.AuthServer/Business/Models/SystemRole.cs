using MicroAngels.Core.Domain;
using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	public class SystemRole
	{
		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public Guid RoleId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public Guid SystemId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string RoleName { get; set; }

		[SugarColumn(IsNullable = true, Length = 2000)]
		public string Description { get; set; }

		//TODO will use view model instead
		[SugarColumn(IsIgnore =true)]
		public Guid UserId { get; set; }


		public static List<ValidateResult> Validate(SystemRole r)
		{
			return
			r
			 .NotNull(r.RoleName, "")
			 .NotNull(r.SystemId,"")
			 .Validate();
		}
	}

}

