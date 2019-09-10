using MicroAngels.Core;
using SqlSugar;
using System;

namespace Business
{

	public class Account
	{
		public Account() { }

		public void ChangePassword(string password)
		{
			Password = password;
		}

		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public Guid Id { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string Name { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string Password { get; set; }

		[SugarColumn(IsNullable = true, Length = 12)]
		public string Phone { get; set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public string Email { get; set; }

		[SugarColumn(IsIgnore = true)]
		public bool IsValidate => !Name.IsNullOrEmpty() && !Password.IsNullOrEmpty();

	}
}
