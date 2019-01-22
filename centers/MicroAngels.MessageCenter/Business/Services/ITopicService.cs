using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface ITopicService
    {
        Task<List<Topic>> GetTopicsAsync(string topic,int pageIndex, int pageSize, out int pageCount);
        Task<Topic> GetTopicAsync(string topic,string serviceId);
        Task<Topic> GetTopicAsync(string topicId);
        Task<bool> EditTopicAsync(Topic topic); // create or update topic async
    }

   
}
