using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface ITopicService
    {
        IEnumerable<Topic> Search(Expression<Func<Topic, bool>> whereExpressions, int? pageIndex, int? pageSize, out int pageCount);
        Task<Topic> GetTopicAsync(string topic,string serviceId);
        Task<Topic> GetTopicAsync(string topicId);
        Task<bool> EditTopicAsync(Topic topic); // create or update topic async
    }

   
}
