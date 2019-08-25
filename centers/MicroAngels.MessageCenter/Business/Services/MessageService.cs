using Business.Helpers;
using DotNetCore.CAP;
using Infrastructure.Orms.Sugar;
using MicroAngels.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{

	public class MessageService : MySqlDbContext, IMessageService, ICapSubscribe
	{

		public MessageService(ITopicService topicService, ISubscribeService subscribeService, ILogger<MessageService> logger)
		{
			_topicService = topicService;
			_subscribeService = subscribeService;
			_logger = logger;
		}

		public async Task<Message> GetMessage(string messageId)
		{
			return await MessageDb.AsQueryable().FirstAsync(m => m.Id == messageId);
		}

		public async Task<List<Message>> Search(MessageSearchOptions options, PageOptions page)
		{
			var query = DB.Queryable<Message, Topic>((m, t) => new object[] {
				JoinType.Inner,m.TopicId == t.Id
			});

			if (!options.topicId.IsNullOrEmpty())
				query.Where((m, t) => m.TopicId == options.topicId);
			if (!options.serviceId.IsNullOrEmpty())
				query.Where((m, t) => m.ServiceId == options.serviceId);
			if (!options.typeId.IsNullOrEmpty())
				query.Where((m, t) => m.TypeId == options.typeId);

			page.TotalCount = query.Count();

			var result = query.Select((m, t) => new Message
			{
				Id = m.Id,
				Topic = t.Name,
				Body = m.Body,
				ReceiveTime = m.ReceiveTime,
				SenderId = m.SenderId,
				SendTime = m.SendTime,
				ServiceId = m.ServiceId,
				StatusId = m.StatusId,
				TopicId = m.TopicId,
				TypeId = m.TypeId,
				Title = m.Title
			});

			return !page.IsValidate ? await result.ToListAsync() : await result.ToPageListAsync(page.PageIndex, page.PageSize);
		}

		public async Task<List<UserMessage>> SearchUserMessage(MessageSearchOptions options, PageOptions page)
		{
			var serviceId = options?.serviceId;
			var typeId = options?.typeId;
			var topicId = options?.topicId;
			var receiverId = options?.reveiverId;
			var statusId = options?.statusId;
			var senderIds = options?.senderIds;

			var query = DB.Queryable<Message, UserMessage>((m, um) => new object[] {
				JoinType.Left,m.Id==um.MessageId && um.ReceiverId==receiverId
			});

			if (!senderIds.IsNull() && senderIds.Length > 0)
				query.In((m, um) => m.SenderId, senderIds);

			if (statusId == StaticKeys.UserMessageStatusId_Waiting)
				query.Where((m, um) => string.IsNullOrEmpty(um.ReceiverId));

			if (!serviceId.IsNullOrEmpty())
				query.Where((m, um) => m.ServiceId == serviceId || um.ServiceId == serviceId);

			if (!typeId.IsNullOrEmpty())
				query.Where((m, um) => m.TypeId == typeId);

			if (!topicId.IsNullOrEmpty())
				query.Where((m, um) => m.TopicId == topicId);

			if (!page.IsNull())
				page.TotalCount = query.Count();

			var result = query.Select((m, um) => new UserMessage
			{
				Id = um.Id,
				ReceiverId = um.ReceiverId,
				MessageId = um.MessageId,
				ServiceId = um.ServiceId,

				Message = m,
			});

			return page.IsNull() || !page.IsValidate ? await result.ToListAsync() : await result.ToPageListAsync(page.PageIndex, page.PageSize);
		}

		[CapSubscribe(AppKeys.MessageCenterSubscribe)]
		public async Task<bool> SubscribeAsync(string message)
		{
			//validate message 
			Message msg = JsonConvert.DeserializeObject<Message>(message);
			if (!msg.IsValidate || msg.SubscriberId.IsNullOrEmpty() || msg.TargetId.IsNullOrEmpty())
			{
				_logger.LogError(AlterKeys.Error.MESSAGE_INVALID, msg.Id);
				return false;
			}

			var topicObj = await _topicService.GetTopicAsync(msg.Topic, msg.ServiceId);
			if (topicObj.IsNull())
			{
				return false;
			}

			msg.Id = Guid.NewGuid().ToString();
			msg.TopicId = topicObj.Id;
			msg.TypeId = StaticKeys.MessageTypeId_Subscribe;
			msg.ReceiveTime = DateTime.UtcNow;
			if (!MessageDb.Insert(msg))
			{
				return false;
			}

			// handle message logic
			var subscribe = new Subscribe(Guid.NewGuid().ToString(), msg.ServiceId, msg.TopicId, msg.SubscriberId, msg.TargetId);
			return await _subscribeService.SubscribeAsync(subscribe);
		}

		[CapSubscribe(AppKeys.MessageCenterNotfiy)]
		public async Task<bool> NotifyAsync(string message)
		{
			Message msg = JsonConvert.DeserializeObject<Message>(message);
			if (!msg.IsValidate)
			{
				return false;
			}

			var topicObj = await _topicService.GetTopicAsync(msg.Topic, msg.ServiceId);
			if (topicObj.IsNull())
			{
				return false;
			}

			msg.Id = Guid.NewGuid().ToString();
			msg.TopicId = topicObj.Id;
			msg.TypeId = StaticKeys.MessageTypeId_Notify;
			msg.ReceiveTime = DateTime.UtcNow;

			return MessageDb.Insert(msg);
		}

		[CapSubscribe(AppKeys.MessageCenterAnnounce)]
		public async Task<bool> AnnounceAsync(string message)
		{
			Message msg = JsonConvert.DeserializeObject<Message>(message);
			return await AnnounceAsync(msg);
		}

		public async Task<bool> AnnounceAsync(Message message)
		{
			if (!message.IsValidate)
			{
				return false;
			}

			var topicObj = await _topicService.GetTopicAsync(message.TopicId);
			if (topicObj.IsNull())
			{
				return false;
			}

			message.Id = Guid.NewGuid().ToString();
			message.TopicId = topicObj.Id;
			message.TypeId = StaticKeys.MessageTypeId_Announce;
			message.SendTime = DateTime.UtcNow;
			message.ReceiveTime = DateTime.UtcNow;

			return MessageDb.Insert(message);
		}

		public Task<bool> AddUserMessage(UserMessage userMessage)
		{
			if (userMessage.IsValidate)
			{
				return Task.FromResult(false);
			}

			var result = UserMessageDb.Insert(new UserMessage
			{
				MessageId = userMessage.MessageId,
				ReceiverId = userMessage.ReceiverId,
			});

			return Task.FromResult(result);
		}

		public async Task<List<UserMessage>> GetUnReadMessage(MessageSearchOptions options)
		{
			options.statusId = StaticKeys.UserMessageStatusId_Waiting;
			var userMessages = await SearchUserMessage(options, null);

			return userMessages;
			//return userMessages?.FindAll(m => m.ReceiverId.IsNullOrEmpty());
		}

		public async Task<bool> ReceiveMessages(MessageSearchOptions options)
		{
			var unreadMessages = await GetUnReadMessage(options);

			foreach (var message in unreadMessages)
			{
				UserMessageDb.Insert(
					new UserMessage
					{
						Id = Guid.NewGuid().ToString(),
						MessageId = message.Message.Id,
						ReceiverId = options?.reveiverId,
						ServiceId = options?.serviceId,
						StatusId = Guid.Empty.ToString()
					});
			}

			return true;
		}

		private readonly ITopicService _topicService;
		private readonly ISubscribeService _subscribeService;
		private readonly ILogger<MessageService> _logger;
	}

}
