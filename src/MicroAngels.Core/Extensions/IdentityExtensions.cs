using System;
using System.Linq;
using System.Security.Claims;

namespace MicroAngels.Core
{

	public static class ClaimsPrincipalExtension
	{

		public static string GetClaimsValue(this ClaimsPrincipal user, string claimType)
		{
			return user.Claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
		}

		public static string GetUserId(this ClaimsPrincipal user) => user.GetClaimsValue(CoreKeys.USER_ID);
		public static string GetServiceId(this ClaimsPrincipal user) => user.GetClaimsValue(CoreKeys.SYSTEM_ID);

		public static T Get<T>(this ClaimsPrincipal user) where T : class => user.GetClaimsValue(CoreKeys.USER).ToObject<T>();
	}

}
