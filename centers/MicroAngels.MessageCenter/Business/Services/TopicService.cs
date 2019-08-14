using Infrastructure.Orms.Sugar;
using MicroAngels.Core;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Services
{

	public class TopicService : MySqlDbContext, ITopicService
	{

		public TopicService(ILogger<TopicService> logger)
		{
			_logger = logger;
		}

		public async Task<bool> EditTopicAsync(Topic topic)
		{
			var result = true;
			topic.EnsureNotNull(() => new ArgumentException());
			var isExists = !topic.Id.IsNullOrEmpty() && !TopicsDb.GetById(topic.Id).IsNull();
			if (isExists)
			{
				// upadate
				result = await TopicsDb.AsUpdateable(topic).ExecuteCommandAsync() > 0;
			}
			else
			{
				topic.Id = Guid.NewGuid().ToString();
				topic.CreateTime = DateTime.UtcNow;

				if (TopicsDb.Count(t => t.Name == topic.Name) > 0)
					result = false;

				// add
				result = await TopicsDb.AsInsertable(topic).ExecuteCommandAsync() > 0;
			}

			return result;
		}


		public Task<List<Topic>> GetTopicsAsync(string topic, int pageIndex, int pageSize, out int pageCount)
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

		public IEnumerable<Topic> Search(Expression<Func<Topic, bool>> whereExpressions, int? pageIndex, int? pageSize, out int pageCount)
		{
			pageCount = 0;
			var query = whereExpressions.IsNull() ? TopicsDb.AsQueryable() : TopicsDb.AsQueryable().Where(whereExpressions);
			if (pageSize.HasValue && pageIndex.HasValue)
			{
				return query.ToPageList(pageIndex.Value, pageSize.Value, ref pageCount);
			}
			else
				return query.ToList();
		}

		private readonly ILogger<TopicService> _logger;

	}

}
