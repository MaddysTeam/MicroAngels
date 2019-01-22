using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Errors
{

    public interface IError
    {
        string Id { get; }
        string Message { get; }
        string Level { get; }
        IList<Exception> Inner { get; }
    }

}
