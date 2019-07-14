﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Models
{

	public class SignupModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}

}
