using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface ITopicService
    {
        Task<List<Topic>> GetTopicsAsync(string serviceId);
        Task<bool> SubscribeAsync(string topicId, string subcriberId, string targetId,string serviceId);
        Task<bool> UnSubsribeAsync(string topicId, string subcriberId, string targetId,string serviceId);
        Task<bool> EditTopicAsync(Topic topic); // create or update topic async
    }

   
}
