using Business.Handlers;
using Business.Models;
using DotNetCore.CAP;
using Infrastructure.Orms.Sugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public class MessageService : IMessageService
    {

        static SqlSugar.SimpleClient<Message> MessageDb => MySqlDbContext.Current.MessageDb;

        public MessageService()
        {

        }

        public Task<bool> CleanAsync(params string[] messageId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CleanByTopicAsync(string topic)
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetMessagesAsync(MessageSearchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserMessage>> GetUserMessagesAsync(string userid, string serviceId, params string[] topicIds)
        {
            throw new NotImplementedException();
        }

        public bool IsExpire(string messageId)
        {
            throw new NotImplementedException();
        }

        public bool IsReceived(string messageId)
        {
            throw new NotImplementedException();
        }

        [CapSubscribe("MessageCenter.Subscribe")]
        public Task<Message> SubscribeAsync(string topic, string body, string serviceId, string senderId, string[] targetIds)
        {
            // 1 ensure topic exists

            // 2 save message

            // 3 handle message logic

            throw new NotImplementedException();
        }

        [CapSubscribe("MessageCenter.Notify")]
        public Task<Message> NotifyAsync(string topic, string body, string serviceId, string senderId)
        {
            throw new NotImplementedException();
        }

        public Task<UserMessage> AddUserMessage(string userId, string messageId, string serviceId)
        {
            throw new NotImplementedException();
        }


        private Dictionary<string, IMessageHandler> _messageHandlers;

    }

}
