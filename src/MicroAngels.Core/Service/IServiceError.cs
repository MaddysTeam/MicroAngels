using MicroAngels.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Service
{

    public interface IServiceError : IError
    {
         string ServiceId { get; set; }
    }

}
