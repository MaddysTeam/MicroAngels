using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Validators
{

    public class EmailValidator : IValidator<string>
    {
        public IValidateResult Validate(ValidateContext<string> context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IValidateResult> Validates(ValidateContext<string> context)
        {
            throw new NotImplementedException();
        }
    }

    public class PhoneNumberValidator : IValidator<string>
    {
        public IValidateResult Validate(ValidateContext<string> context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IValidateResult> Validates(ValidateContext<string> context)
        {
            throw new NotImplementedException();
        }
    }

}
