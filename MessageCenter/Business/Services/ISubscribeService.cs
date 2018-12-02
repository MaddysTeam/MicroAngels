using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface ISubscribeService
    {
        Task<bool> SubscribeAsync(Subscribe subscribe);
        Task<bool> UnSubsribeAsync(Subscribe subscribe);
        Task<List<string>> GetSubscribers(string targetId, string serviceId, string topicId);
    }

   
}
