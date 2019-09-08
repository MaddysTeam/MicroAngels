using MicroAngels.Core;
using SqlSugar;
using System;

namespace Business
{

	public class Account
	{
		public Account() { }

		public Account(Guid id, string name, string password)
		{
			Id = id;
			Name = name;
			Password = password;
		}

		public void ChangePassword(string password)
		{
			Password = password;
		}

		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public Guid Id { get; private set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string Name { get; private set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string Password { get; private set; }

		[SugarColumn(IsNullable = true, Length = 12)]
		public string Phone { get; private set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public string Email { get; private set; }

		[SugarColumn(IsIgnore = true)]
		public bool IsValidate => !Id.IsEmpty() && !Name.IsNullOrEmpty() && !Password.IsNullOrEmpty();

	}
}
