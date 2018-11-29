using Business;
using Business.Handlers;
using Business.Models;
using Common;
using DotNetCore.CAP;
using Infrastructure;
using Infrastructure.Orms.Sugar;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public class TopicService : ITopicService
    {
        static SqlSugar.SimpleClient<Topic> TopicDb => MySqlDbContext.Current.TopicsDb;
        static SqlSugar.SimpleClient<Suscribe> SuscribeDb;

        public TopicService(
           ILogger<TopicService> logger,
           CAPMysqlDbContext context,
           ICapPublisher serviceBus
           )
        {
            _logger = logger;
            _dbContext = context;
            _serviceBus = serviceBus;
            //_topicHandlers = topicHandlers;
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

        public Task<bool> SubscribeAsync(string topicId, string subcriberId, string targetId, string serviceId)
        {
            var topic = TopicDb.GetById(topicId);
            if (topic.IsNull()) return Task.FromResult(false);

            SuscribeDb.Insert(new Suscribe { ServiceId=serviceId, SubscriberId=subcriberId, TargetId =targetId, TopicId=topicId });

            throw new NotImplementedException();
        }

        public Task<bool> UnSubsribeAsync(string topicId, string subcriberId, string targetId, string serviceId)
        {
            var topic = TopicDb.GetById(topicId);

            throw new NotImplementedException();
        }

        private readonly ILogger<TopicService> _logger;
        private readonly CAPMysqlDbContext _dbContext;
        private readonly ICapPublisher _serviceBus;

    }

}
