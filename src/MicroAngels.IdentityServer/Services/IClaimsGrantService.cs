using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Services
{

	public interface IClaimsGrantService
	{
		Task<Claim[]> GetClaims();
	}

}
