using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	public class UserRole
	{
		[SugarColumn(IsPrimaryKey = true,Length = 50)]
		public Guid Id { get; set; }

		[SugarColumn(IsNullable =false, Length = 50)]
		public Guid UserId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public Guid RoleId { get; set; }

		public static List<ValidateResult> Validate(UserRole ur)
		{
			return
			ur.NotNull(ur.UserId, "")
			 .NotNull(ur.RoleId, "")
			 .Validate();
		}
	}

}
