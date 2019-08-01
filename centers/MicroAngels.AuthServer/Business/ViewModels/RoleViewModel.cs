using System;

namespace Business
{

	public class RoleViewModel
	{
		public Guid Id { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }
		public Guid SystemId { get; set; }
	}

}
