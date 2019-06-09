﻿using MicroAngels.Core.Plugins;
using System;
using System.Collections.Generic;

namespace UserService.Business
{
	/// <summary>
	/// 用户
	/// </summary>
	public class UserInfo
	{
		public Guid UserId { get;  set; }
		public string UserName { get;  set; }
		public int Gender { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public IEnumerable<SystemRole> UserRoles { get; set; }

		public static List<ValidateResult> Validate(UserInfo u)
		{
			return
			u.NotNull(u.UserId, "")
			 .NotNull(u.UserName, "")
			 .Length(u.Phone,11,"")
			 .Validate();
		}
	}

}
