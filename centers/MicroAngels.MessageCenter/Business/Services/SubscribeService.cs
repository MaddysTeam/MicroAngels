using Infrastructure.Orms.Sugar;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Services
{

	public class SubscribeService : MySqlDbContext, ISubscribeService
	{

		public SubscribeService(IServiceFinder<ConsulService> serviceFinder, ILoadBalancer loadBalancer, ITopicService topicService, IConfiguration conf)
		{
			_serviceFinder = serviceFinder;
			_loadBalancer = loadBalancer;
			_topicServce = topicService;
			_conf = conf;
		}

		public async Task<bool> SubscribeAsync(Subscribe sub)
		{
			if (!sub.IsValidate)
			{
				return false;
			}

			var topic = await _topicServce.GetTopicAsync(sub.TopicId);
			if (topic.IsNull()) return false;

			//再判断下是否已经订阅过

			var searchOptions = new SubscribeSearchOptions { ServiceId = sub.ServiceId, SubscriberId = sub.SubscriberId, TopicId = sub.TopicId, TargetId = sub.TargetId };
			var existSubscribes = await Search(searchOptions, null);
			if (!existSubscribes.IsNull() && existSubscribes.Count > 0)
			{
				return false;
			}

			sub.Id = Guid.NewGuid().ToString();
			return SubscribeDb.Insert(sub);
		}

		public async Task<bool> UnSubsribeAsync(Subscribe sub)
		{
			var topic = await _topicServce.GetTopicAsync(sub.TopicId);
			if (topic.IsNull()) return false;

			var searchOptions = new SubscribeSearchOptions { ServiceId = sub.ServiceId, SubscriberId = sub.SubscriberId, TopicId = sub.TopicId, TargetId = sub.TargetId };
			var existSubscribes = await Search(searchOptions, null);
			if (existSubscribes.IsNull() || existSubscribes.Count() <= 0) return false;

			try
			{
				return SubscribeDb.DeleteById(existSubscribes.First().Id.ToGuid());
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public async Task<List<Subscribe>> Search(SubscribeSearchOptions options, PageOptions page)
		{
			var query = DB.Queryable<Subscribe>();

			if (!options.IsNull())
			{

				var SubscriberId = options?.SubscriberId;
				var TargetId = options?.TargetId;
				var ServiceId = options?.ServiceId;
				var TopicId = options?.TopicId;
				var code = options?.Code;


				if (!SubscriberId.IsNullOrEmpty())
					query.Where(s => s.SubscriberId == SubscriberId);
				if (!TargetId.IsNullOrEmpty())
					query.Where(s => s.TargetId == TargetId);
				if (!ServiceId.IsNullOrEmpty())
					query.Where(s => s.ServiceId == ServiceId);
				if (!TopicId.IsNullOrEmpty())
					query.Where(s => s.TopicId == TopicId);
			}

			var targets = new List<Subscribe>();
			if (!page.IsNull() && page.IsValidate)
				targets = await query.ToPageListAsync(page.PageIndex, page.PageSize);
			else
			{
				targets = await query.ToListAsync();
			}

			return targets;
		}


		private IServiceFinder<ConsulService> _serviceFinder;
		private ILoadBalancer _loadBalancer;
		private ITopicService _topicServce;
		private IConfiguration _conf;

	}

}
