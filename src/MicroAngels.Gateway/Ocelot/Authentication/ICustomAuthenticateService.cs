using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public interface ICustomAuthenticateService
	{
		Task<bool> ValidateAuthenticate(HttpContext context, string path);
	}

}
