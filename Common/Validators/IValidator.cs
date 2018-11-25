using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Validators
{

    public interface IValidator<T>
    {
        IValidateResult Validate(ValidateContext<T> context);
        IEnumerable<IValidateResult> Validates(ValidateContext<T> context);
    }

    public interface IValidateResult
    {
        bool IsValid { get; }
        string Message { get; }
    }

}
