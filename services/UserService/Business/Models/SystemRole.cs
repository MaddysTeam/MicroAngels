using MicroAngels.Core.Domain;
using System;

namespace UserService.Business
{

	public class SystemRole:IAggregateRoot
	{
		public Guid RoleId { get; set; }
		public Guid SystemId { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }

		public string Id => RoleId.ToString();
	}

}

