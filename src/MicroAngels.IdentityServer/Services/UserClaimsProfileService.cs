using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Services
{

	public class UserClaimsProfileService : IProfileService
	{

		public Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			context.IssuedClaims = context.Subject.Claims.ToList();

			return Task.CompletedTask;
		}

		public Task IsActiveAsync(IsActiveContext context)
		{
			context.IsActive = true;
			return Task.CompletedTask;
		}

	}

}
