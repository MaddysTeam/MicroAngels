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

			var searchOptions = new SubscribeSearchOptions { serviceId = sub.ServiceId, subscriberId = sub.SubscriberId, topicId = sub.TopicId, targetId = sub.TargetId };
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

			var searchOptions = new SubscribeSearchOptions { serviceId = sub.ServiceId, subscriberId = sub.SubscriberId, topicId = sub.TopicId, targetId = sub.TargetId };
			var existSubscribes = await Search(searchOptions, null);
			if (existSubscribes.IsNull() || existSubscribes.Count() <= 0) return false;

			try
			{
				return SubscribeDb.DeleteById(existSubscribes.First().Id.ToGuid());
			}
			catch(Exception e)
			{
				return false;
			}
		}

		public async Task<List<Subscribe>> Search(SubscribeSearchOptions options, PageOptions page)
		{

			var subscriberId = options?.subscriberId;
			var targetId = options?.targetId;
			var serviceId = options?.serviceId;
			var topicId = options?.topicId;
			var code = options?.code;

			var query = DB.Queryable<Subscribe>();
			if (!subscriberId.IsNullOrEmpty())
				query.Where(s => s.SubscriberId == subscriberId);
			if (!targetId.IsNullOrEmpty())
				query.Where(s => s.TargetId == targetId);
			if (!serviceId.IsNullOrEmpty())
				query.Where(s => s.ServiceId == serviceId);
			if (!topicId.IsNullOrEmpty())
				query.Where(s => s.TopicId == topicId);

			var targets = new List<Subscribe>();
			if (!page.IsNull() && page.IsValidate)
				targets = await query.ToPageListAsync(page.PageIndex, page.PageSize);
			else
			{
				try
				{
					targets = await query.ToListAsync();
				}
				catch (Exception e)
				{

				}

			}

			if (options.IsJoinUser)
			{
				if (targets.Count < 0)
					return new List<Subscribe>();

				var users = new List<UserViewModel>();
				using (var client = new HttpClient())
				{
					var fromService = _conf["UserServcie:From"];
					var virtualPath = _conf["VirtualPath"];
					users = await client.PostAsync<List<UserViewModel>, ConsulService>(fromService, virtualPath, code, null, _serviceFinder, _loadBalancer);
				}

				return targets.Select(t =>
				{
					var user = users.Find(u => u.Id.ToString() == t.TargetId);
					if (!user.IsNull())
					{
						t.Target = user.UserName;
						t.Subscriber = user.RealName;
					}

					return t;
				}).ToList();
			}


			return targets;
		}


		private IServiceFinder<ConsulService> _serviceFinder;
		private ILoadBalancer _loadBalancer;
		private ITopicService _topicServce;
		private IConfiguration _conf;

	}

}
