using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface IMessageService
    {
        Task<Message> SubscribeAsync(string topic, string body,string serviceId,string senderId, string[] targetIds);
        Task<Message> NotifyAsync(string topic,string body, string serviceId, string senderId);
        bool IsReceived(string messageId);
        Task<List<Message>> GetMessagesAsync(MessageSearchOptions options);
        bool IsExpire(string messageId);
        Task<bool> CleanByTopicAsync(string topic);
        Task<bool> CleanAsync(params string[] messageId);
        Task<List<UserMessage>> GetUserMessagesAsync(string userid,string serviceId,params string[] topicIds);
        Task<UserMessage> AddUserMessage(string userId,string messageId,string serviceId);
    }

}
