﻿using System.Linq;
using System.Security.Claims;

namespace MicroAngels.Core
{

	public static class ClaimsPrincpleExtension
	{
		public static string GetClaimsValue(this ClaimsPrincipal user,string claimType)
		{
			return user.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
		}
	}

}
