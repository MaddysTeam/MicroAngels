using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Handlers
{

    public interface IMessageHandler
    {
        Task HandleMessageAsync(Message message);
    }

}
