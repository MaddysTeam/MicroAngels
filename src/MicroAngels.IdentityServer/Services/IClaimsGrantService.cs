using IdentityServer4.Validation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Services
{

	public interface IClaimsGrantService
	{
		Task<Claim[]> GetClaims(ResourceOwnerPasswordValidationContext context);
	}

}
