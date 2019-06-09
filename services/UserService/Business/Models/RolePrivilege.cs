using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Business
{
	public class RolePrivilege
	{
		public Guid Id { get; set; }
		public Guid RoleId { get; set; }
		public Guid PrivilegeId { get; set; }
	}
}
