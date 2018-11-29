using Business;
using Business.Models;
using Common;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter.Business.Services
{

    public class TopicService : ITopicService
    {
        static SqlSugar.SimpleClient<Topic> TopicDb => MySqlDbContext.Current.TopicsDb;

        public TopicService(
           ILogger<TopicService> logger,
           CAPMysqlDbContext context,
           ICapPublisher serviceBus
           )
        {
            _logger = logger;
            _dbContext = context;
            _serviceBus = serviceBus;
        }

        public async Task<bool> EditTopicAsync(Topic topic)
        {
            topic.EnsureNotNull(() => new ArgumentException());
            var isExists = !TopicDb.GetById(topic.TopicId).IsNull();
            if (isExists)
            {
                // upadate
                MySqlDbContext.Current.TopicsDb.Update(t => topic, t => t.TopicId == topic.TopicId);
            }
            else
            {
                if (TopicDb.Count(t => t.Name == topic.Name) > 0)
                    return false;

                // add
                MySqlDbContext.Current.TopicsDb.Insert(topic);
            }

            return true;
        }

        public async Task<List<Topic>> GetTopicsAsync(string serviceId)
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeAsync(string topicId, string subcriberId, string targetId)
        {
            throw new NotImplementedException();
        }

        public async Task UnSubsribeAsync(string topicId, string subcriberId, string targetId)
        {
            throw new NotImplementedException();
        }


        private readonly ILogger<TopicService> _logger;
        private readonly CAPMysqlDbContext _dbContext;
        private readonly ICapPublisher _serviceBus;

    }

}
