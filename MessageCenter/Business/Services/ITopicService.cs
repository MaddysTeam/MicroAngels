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
        Task SubscribeAsync(string topicId, string subcriberId, string targetId);
        Task UnSubsribeAsync(string topicId, string subcriberId, string targetId);
        //Task<bool> AddTopicAsync(string serviceId,string topic, string desc); // create or update topic async
        Task<bool> EditTopicAsync(Topic topic); // create or update topic async
    }

}
