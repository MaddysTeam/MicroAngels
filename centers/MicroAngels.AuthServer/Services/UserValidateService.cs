using MicroAngels.IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class UserValidateService : IUserValidateService
	{

		public Task<bool> ValidatePassword(string username, string password)
		{
			return Task.FromResult(true);
		}

	}
}
