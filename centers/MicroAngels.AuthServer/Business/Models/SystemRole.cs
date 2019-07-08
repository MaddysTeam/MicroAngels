using MicroAngels.Core.Domain;
using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	public class SystemRole
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid RoleId { get; set; }
		public Guid SystemId { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }


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

