using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Services
{
	public interface IUserValidateService
	{

		Task<bool> ValidatePassword(ResourceOwnerPasswordValidationContext context);

	}


}
