using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IMessageService
    {
        Task<Message> ReceiveAsync(string serviceId,string topicId, string typeId, string body, string senderId, string levelId);
        bool IsReceived(string messageId);
        Task<List<Message>> GetMessagesAsync(MessageSearchOptions options);
        bool IsExpire(string messageId);
        Task<bool> CleanByTopicAsync(string topic);
        Task<bool> CleanAsync(params string[] messageId);
        Task<List<UserMessage>> GetUserMessageAsync(string userid,string serviceId,params string[] topicIds);
    }

}
