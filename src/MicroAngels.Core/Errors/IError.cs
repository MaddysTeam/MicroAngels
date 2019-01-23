using System;
using System.Collections.Generic;

namespace MicroAngels.Core
{

    public interface IError
    {
        string Id { get; }
        string Message { get; }
        string Level { get; }
        IList<Exception> Inner { get; }
    }

}
