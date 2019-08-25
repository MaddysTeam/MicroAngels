using System;

namespace Business
{

	public class UserViewModel
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string HeaderImagePath { get; set; }
	}

}
