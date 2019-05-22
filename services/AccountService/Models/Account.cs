using System;

namespace AccountService.Models
{
	public class Account
	{
		public Account(long id,string name, DateTime birthday)
		{
			Id = id;
			UserName = name;
			Birthday = birthday;
		}

		public long Id { get; private set; }
		public string UserName { get; private set; }
		public string Password { get; set; }
		public DateTime Birthday { get; private set; }
		public string Email { get; private set; }
		public string Question { get; private set; }
		public string RealName { get; private set; }
		public string IDCard { get; private set; }

	}
}
