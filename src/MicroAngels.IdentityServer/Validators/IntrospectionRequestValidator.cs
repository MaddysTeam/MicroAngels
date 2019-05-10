using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Validators
{
	class IntrospectionRequestValidator : IIntrospectionRequestValidator
	{
		public Task<IntrospectionRequestValidationResult> ValidateAsync(NameValueCollection parameters, ApiResource api)
		{
			throw new NotImplementedException();
		}

	}
}
