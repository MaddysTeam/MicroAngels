using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Infrastructure.Orms.Sugar;
using MicroAngels.Core;
using Microsoft.Extensions.Logging;

namespace Business.Services
{

    public class SubscribeService : MySqlDbContext, ISubscribeService
    {
        //static SqlSugar.SimpleClient<Topic> TopicDb => MySqlDbContext.Current.TopicsDb;
        //static SqlSugar.SimpleClient<Subscribe> SubscribeDb => MySqlDbContext.Current.SubscribeDb;

        public SubscribeService(ILogger<TopicService> logger)
        {
            _logger = logger;
        }

        public Task<bool> SubscribeAsync(Subscribe sub)
        {
            var result = true;
            if (sub.IsNull() || sub.TopicId.IsNullOrEmpty()
                             || sub.SubscriberId.IsNullOrEmpty()
                             || sub.ServiceId.IsNullOrEmpty()
                             || sub.TargetId.IsNullOrEmpty())
            {
                result = false;
            }

            var topic = TopicsDb.GetById(sub.TopicId);
            if (topic.IsNull()) return Task.FromResult(result);

            //TODO:再判断下是否已经订阅过

            result = SubscribeDb.Insert(sub);

            return Task.FromResult(result);
        }

        public Task<bool> UnSubsribeAsync(Subscribe subscribe)
        {
            var result = false;
            var topicObj = TopicsDb.GetById(subscribe.TopicId);
            if (topicObj.IsNull()) return Task.FromResult(false);

            result = SubscribeDb.Delete(sub => sub.TopicId == subscribe.TopicId
                                    && sub.SubscriberId == subscribe.SubscriberId
                                    && sub.TargetId == subscribe.TargetId
                                    && sub.ServiceId == subscribe.ServiceId);

            return Task.FromResult(result);
        }

        public Task<List<Subscribe>> GetSubscribes(string targetId, string serviceId, string topicId, int pageIndex, int pageSize, out int pageCount)
        {
            var query = DB.Queryable<Subscribe>();
            if (!targetId.IsNullOrEmpty())
                query.Where(s => s.TargetId == targetId);
            if (!serviceId.IsNullOrEmpty())
                query.Where(s => s.ServiceId == serviceId);
            if (!topicId.IsNullOrEmpty())
                query.Where(s => s.TopicId == topicId);

            pageCount = query.Count();

            return  pageSize<=0 ? query.ToListAsync():query.ToPageListAsync(pageIndex,pageSize);
        }

        private readonly ILogger<TopicService> _logger;

    }

}
