using Business.Models;
using Common;
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

        //SimpleClient<Message> MessageDb => MySqlDbContext.Current.MessageDb;
        //SimpleClient<UserMessage> UserMessageDb => MySqlDbContext.Current.UserMessageDb;
        //SqlSugarClient db => MySqlDbContext.Current.DB;

        public MessageService(ITopicService topicService, ILogger<MessageService> logger)
        {
            _topicService = topicService;
            _logger = logger;
        }

        public Task<Message> GetMessage(string messageId)
        {
            return Task.FromResult(MessageDb.GetById(messageId));
        }

        public Task<List<Message>> GetMessagesAsync(string topic, string serviceId, string typeId, int pageIndex, int pageSize, out int pageCount)
        {
            var query = MySqlDbContext.Current.DB.Queryable<Message>();
            if (!topic.IsNullOrEmpty())
                query.Where(it => it.Topic == topic);
            if (!serviceId.IsNullOrEmpty())
                query.Where(it => it.ServiceId == serviceId);

            pageCount = query.Count();

            return query.ToPageListAsync(pageIndex, pageSize);
        }

        public Task<List<Message>> GetUserMessagesAsync(string userid, string serviceId, string typeId, int pageIndex, int pageSize, out int pageCount)
        {
            var query = MySqlDbContext.Current.DB.Queryable<Message, UserMessage>((m, um) => new object[] {
                JoinType.Left,m.Id==um.MessageId
            })
            .Where((m, um) => um.ReceiverId == userid);

            if (!serviceId.IsNullOrEmpty())
                query.Where((m, um) => m.ServiceId == serviceId);
            if (!typeId.IsNullOrEmpty())
                query.Where((m, um) => m.TypeId == typeId);

            query.Select((m, um) => m);

            pageCount = query.Count();

            return query.ToPageListAsync(pageIndex, pageSize);
        }

        public Task<bool> AddUserMessage(string userId, string messageId, string serviceId)
        {
            var result = UserMessageDb.Insert(new UserMessage
            {
                MessageId = messageId,
                ReceiverId = userId,
            });

            return Task.FromResult(result);
        }


        [CapSubscribe(AppKeys.MessageCenterSubscribe)]
        public async Task SubscribeAsync(string message)
        {
            //validate message 
            Message msg = JsonConvert.DeserializeObject<Message>(message);
            if (!msg.IsValidate || msg.SubscriberId.IsNullOrEmpty() || msg.TargetId.IsNullOrEmpty())
            {
                _logger.LogError(AlterKeys.Error.MESSAGE_INVALID, msg.Id);
                return;
            }

            var topicObj = await _topicService.GetTopicAsync(msg.Topic, msg.ServiceId);
            if (topicObj.IsNull())
            {
                return;
            }

            msg.Id = Guid.NewGuid().ToString();
            msg.TopicId = topicObj.Id;
            msg.TypeId = StaticKeys.MessageTypeId_Subscribe;
            var result = DB.Insertable(msg).InsertColumns(m => new { m.Id, m.TopicId, m.TypeId, m.Body, m.SenderId, m.ServiceId, m.SendTime }).ExecuteReturnEntity() != null;
            if (!result)
            {
                return;
            }

            // handle message logic
            var subscribe = new Subscribe(Guid.NewGuid().ToString(), msg.ServiceId, msg.TopicId, msg.SubscriberId, msg.TargetId);
            await _subscribeService.SubscribeAsync(subscribe);
        }

        [CapSubscribe(AppKeys.MessageCenterNotfiy)]
        public async Task NotifyAsync(string message)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(message);
            if (!msg.IsValidate)
            {
                return;
            }

            var topicObj = await _topicService.GetTopicAsync(msg.Topic, msg.ServiceId);
            if (topicObj.IsNull())
            {
                return;
            }

            msg.Id = Guid.NewGuid().ToString();
            msg.TopicId = topicObj.Id;
            msg.TypeId = StaticKeys.MessageTypeId_Announce;
            var result = DB.Insertable(msg).InsertColumns(m => new { m.Id, m.TopicId, m.TypeId, m.Body, m.SenderId, m.ServiceId, m.SendTime }).ExecuteReturnEntity() != null;
            if (!result)
            {
                return;
            }

            // 找出所有当前message订阅者，然后将当前message插入user message
            var receiverIds = await _subscribeService.GetSubscribers(msg.TargetId, msg.ServiceId, msg.TopicId);
            List<UserMessage> userMessages = new List<UserMessage>();
            foreach(var receiverId in receiverIds)
            {
                userMessages.Add(new UserMessage  { MessageId=msg.Id, ReceiverId= receiverId, StatusId= StaticKeys.UserMessageStatusId_Waiting });
            }

            UserMessageDb.InsertRange(userMessages.ToArray());
        }

        [CapSubscribe(AppKeys.MessageCenterAnnounce)]
        public async Task AnnounceAsync(string message)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(message);
            if (!msg.IsValidate)
            {
                return;
            }

            var topicObj = await _topicService.GetTopicAsync(msg.Topic, msg.ServiceId);
            if (topicObj.IsNull())
            {
                return;
            }

            msg.Id = Guid.NewGuid().ToString();
            msg.TopicId = topicObj.Id;
            msg.TypeId = StaticKeys.MessageTypeId_Announce;
            var result = DB.Insertable(msg).InsertColumns(m => new { m.Id, m.TopicId, m.TypeId, m.Body, m.SenderId, m.ServiceId, m.SendTime }).ExecuteReturnEntity() != null;
            if (!result)
            {
                return;
            }

        }

        private readonly ITopicService _topicService;
        private readonly ISubscribeService _subscribeService;
        private readonly ILogger<MessageService> _logger;
    }

}
