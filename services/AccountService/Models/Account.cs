using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		public DateTime Birthday { get; private set; }
	}
}
