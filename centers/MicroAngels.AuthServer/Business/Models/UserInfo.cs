using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{
	/// <summary>
	/// 用户
	/// </summary>
	public class UserInfo
	{

		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public Guid UserId { get;  set; }

		[SugarColumn(IsNullable = false, Length = 100)]
		public string UserName { get;  set; }

		public int Gender { get; set; }

		[SugarColumn(IsNullable = true, Length = 100)]
		public string RealName { get; set; }

		[SugarColumn(IsNullable = true, Length = 11)]
		public string Phone { get; set; }

		[SugarColumn(IsNullable = true, Length = 255)]
		public string Email { get; set; }

		[SugarColumn(IsNullable = true, Length = 500)]
		public string HeaderImagePath { get; set; }

		public static List<ValidateResult> Validate(UserInfo u)
		{
			return
			u
			 .NotNull(u.UserName, "")
			 .Length(u.Phone,11,"")
			 .Validate();
		}

	}

}
