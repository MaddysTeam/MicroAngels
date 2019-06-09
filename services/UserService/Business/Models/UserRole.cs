using MicroAngels.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Business
{

	public class UserRole
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid RoleId { get; set; }

		public IEnumerable<SystemPrivilege> Privileges { get; set; }


		public static List<ValidateResult> Validate(UserRole ur)
		{
			return
			ur.NotNull(ur.UserId, "")
			 .NotNull(ur.RoleId, "")
			 .Validate();
		}
	}

}
