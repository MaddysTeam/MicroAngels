using Business.Models;
using MicroAngels.Core;
using DotNetCore.CAP;
using Infrastructure.Orms.Sugar;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Helpers;

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

        public Task<Message> GetMessage(string messageId)
        {
            return Task.FromResult(MessageDb.GetById(messageId));
        }

        public Task<List<Message>> GetMessagesAsync(string topic, string serviceId, string typeId, int pageIndex, int pageSize, out int pageCount)
        {
            var query = DB.Queryable<Message, Topic>((m, t) => new object[] {
                JoinType.Inner,m.TopicId == t.Id
            });

            if (!topic.IsNullOrEmpty())
                query.Where((m, t) => t.Name == topic);
            if (!serviceId.IsNullOrEmpty())
                query.Where((m, t) => m.ServiceId == serviceId);
            if (!typeId.IsNullOrEmpty())
                query.Where((m, t) => m.TypeId == typeId);

            pageCount = query.Count();

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
                TypeId = m.TypeId
            });

            return pageSize <= 0 ? result.ToListAsync() : result.ToPageListAsync(pageIndex, pageSize);
        }

        public Task<List<UserMessage>> GetUserMessagesAsync(string userid, string serviceId, string topicId, string typeId, int pageIndex, int pageSize, out int pageCount)
        {
            var query = DB.Queryable<Message, UserMessage>((m, um) => new object[] {
                JoinType.Left,m.Id==um.MessageId
            })
            .Where((m, um) => um.ReceiverId == userid);

            if (!serviceId.IsNullOrEmpty())
                query.Where((m, um) => m.ServiceId == serviceId || um.ServiceId == serviceId);
            if (!typeId.IsNullOrEmpty())
                query.Where((m, um) => m.TypeId == typeId);
            if (!topicId.IsNullOrEmpty())
                query.Where((m, um) => m.TopicId == topicId);

            pageCount = query.Count();

            var result = query.Select((m, um) => new UserMessage
            {
                Id = um.Id,
                ReceiverId = um.ReceiverId,
                MessageId = um.MessageId,
                ServiceId = um.ServiceId,
                Message = m,
            });

            return pageSize <= 0 ? result.ToListAsync() : result.ToPageListAsync(pageIndex, pageSize);
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
            if (!MessageDb.Insert(msg))
            {
                return false;
            }

            // 找出所有当前message订阅者，然后将当前message插入user message,注意订阅对象是senderid
            var subscribeCount = 0;
            var subscribes = await _subscribeService.GetSubscribes(msg.SenderId, msg.ServiceId, msg.TopicId, 0, 0, out subscribeCount);
            List<UserMessage> userMessages = new List<UserMessage>();
            foreach (var item in subscribes)
            {
                var um = new UserMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageId = msg.Id,
                    ReceiverId = item.SubscriberId,
                    ServiceId = msg.ServiceId,
                    StatusId = StaticKeys.UserMessageStatusId_Waiting
                };

                if (um.IsValidate)
                    userMessages.Add(um);
            }

            return UserMessageDb.InsertRange(userMessages.ToArray());
        }

        [CapSubscribe(AppKeys.MessageCenterAnnounce)]
        public async Task<bool> AnnounceAsync(string message)
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
            msg.TypeId = StaticKeys.MessageTypeId_Announce;
            msg.ReceiveTime = DateTime.UtcNow;

            return MessageDb.Insert(msg);
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

        private readonly ITopicService _topicService;
        private readonly ISubscribeService _subscribeService;
        private readonly ILogger<MessageService> _logger;
    }

}
