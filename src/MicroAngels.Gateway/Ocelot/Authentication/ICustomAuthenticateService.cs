using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public interface ICustomAuthenticateService
	{
		Task<bool> ValidateAuthenticate(string clientId, string path);
	}

	public class CustomAuthenticateService : ICustomAuthenticateService
	{
		public  Task<bool> ValidateAuthenticate(string clientId, string path)
		{
			return Task.FromResult(true);
		}
	}

}
