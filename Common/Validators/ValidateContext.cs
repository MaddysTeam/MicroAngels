using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Validators
{

    public class ValidateContext<ValidateObject>
    {
        ValidateObject Target { get; }

        public ValidateContext(ValidateObject target)
        {
            Target = target;
        }

    }

}
