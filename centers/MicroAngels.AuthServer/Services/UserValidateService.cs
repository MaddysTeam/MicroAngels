using IdentityServer4.Validation;
using MicroAngels.IdentityServer.Services;
using System.Threading.Tasks;

namespace MicroAngels.AuthServer.Services
{

	public class UserValidateService : IUserValidateService
	{

		/// <summary>
		///  validate password sercice when been invoked in ResourceOwnerPasswordValidator
		/// </summary>
		/// <param name="ResourceOwnerPasswordValidationContext"></param>
		/// <returns></returns>
		public Task<bool> ValidatePassword(ResourceOwnerPasswordValidationContext context)
		{
			var username = context.UserName;
			var password = context.Password;

			// you can implement you own validate password here

			return Task.FromResult(true);
		}

	}
}
