using Business.Models;
using MicroAngels.Core;
using Infrastructure.Orms.Sugar;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{

    public class TopicService : MySqlDbContext, ITopicService
    {
        //SimpleClient<Topic> TopicDb => MySqlDbContext.Current.TopicsDb;
        //SimpleClient<Subscribe> SubscribeDb => MySqlDbContext.Current.SubscribeDb;

        public TopicService(ILogger<TopicService> logger)
        {
            _logger = logger;
        }

        public Task<bool> EditTopicAsync(Topic topic)
        {
            var result = true;
            topic.EnsureNotNull(() => new ArgumentException());
            var isExists =!topic.Id.IsNullOrEmpty() && !TopicsDb.GetById(topic.Id).IsNull();
            if (isExists)
            {
                // upadate
                result = TopicsDb.Update(t => topic, t => t.Id == topic.Id);
            }
            else
            {
                topic.Id = Guid.NewGuid().ToString();
                topic.CreateTime = DateTime.UtcNow;

                if (TopicsDb.Count(t => t.Name == topic.Name) > 0)
                    result = false;

                // add
                result = MySqlDbContext.Current.TopicsDb.Insert(topic);
            }

            return Task.FromResult(result);
        }


        public Task<List<Topic>> GetTopicsAsync(string topic,int pageIndex, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> GetTopicAsync(string topic, string serviceId)
        {
            var topicObj = TopicsDb.GetSingle(t => topic == t.Name && t.ServiceId == serviceId);

            return Task.FromResult(topicObj);
        }

        public Task<Topic> GetTopicAsync(string topicId)
        {
            var topic = TopicsDb.GetById(topicId);

            return Task.FromResult(topic);
        }

        private readonly ILogger<TopicService> _logger;

    }

}
