using MicroAngels.Core.Plugins;
using System;
using System.Collections.Generic;

namespace Business
{

	public class UserRole
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
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
