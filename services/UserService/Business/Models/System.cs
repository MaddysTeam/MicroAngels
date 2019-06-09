using MicroAngels.Core.Domain;
using System;
using System.Collections.Generic;

namespace UserService.Business
{
	/// <summary>
	/// 系统
	/// </summary>
	public class System
	{

		public Guid SystemId { get; set; }
		public string SystemName { get; set; }
		public string Code { get; set; }
		public long Version { get; set; }
		public string Description { get; set; }
		public Guid CreatorId { get; set; }
		public DateTime CreateTime { get; set; }
		public Guid ParentId { get; set; }

		public string Id => SystemId.ToString();


		public IEnumerable<UserInfo> Users { get; }

		public IEnumerable<SystemRole> Roles { get; }

		public IEnumerable<SystemPrivilege> Privileges { get; }
	}

}
