using MicroAngels.Core.Domain;
using System;

namespace UserService.Business
{

	public class SystemPrivilege
	{
		public Guid PrivilegeId { get; set; }
		public Guid SystemId { get; set; }
		public string PrivilegeName { get; set; }
		public string Description { get; set; }

		public string Id => PrivilegeId.ToString();
	}

}
