using Infrastructure.Orms.Sugar;
using MicroAngels.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public async Task<bool> SubscribeAsync(Subscribe sub)
        {
            if (sub.IsNull() || sub.TopicId.IsNullOrEmpty()
                             || sub.SubscriberId.IsNullOrEmpty()
                             || sub.ServiceId.IsNullOrEmpty()
                             || sub.TargetId.IsNullOrEmpty())
            {
                return false;
            }

            var topic = TopicsDb.GetById(sub.TopicId);
			if (topic.IsNull()) return false;

			//再判断下是否已经订阅过

			var existSubscribes = await GetSubscribes(s=>s.ServiceId==sub.ServiceId && s.TargetId==sub.TargetId && s.TopicId==sub.TopicId && s.SubscriberId==sub.SubscriberId,null,null);
			if(!existSubscribes.IsNull() && existSubscribes.Count > 0)
			{
				return false;
			}

			return SubscribeDb.Insert(sub);
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

        public async Task<List<Subscribe>> GetSubscribes(Expression<Func<Subscribe, bool>> whereExpressions, int? pageIndex, int? pageSize)
        {
            //var query = DB.Queryable<Subscribe>();
            //if (!targetId.IsNullOrEmpty())
            //    query.Where(s => s.TargetId == targetId);
            //if (!serviceId.IsNullOrEmpty())
            //    query.Where(s => s.ServiceId == serviceId);
            //if (!topicId.IsNullOrEmpty())
            //    query.Where(s => s.TopicId == topicId);

			var query = DB.Queryable<Subscribe>().Where(whereExpressions);

			if (pageSize.HasValue && pageIndex.HasValue)
				return await query.ToPageListAsync(pageIndex.Value, pageSize.Value);
			else
				return await query.ToListAsync();
        }

        private readonly ILogger<TopicService> _logger;

    }

}
