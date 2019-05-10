using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Services
{
	public interface IUserValidateService
	{

		Task<bool> ValidatePassword(string username,string password);

	}


}
