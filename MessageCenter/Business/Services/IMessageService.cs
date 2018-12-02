using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface IMessageService
    {
        Task SubscribeAsync(string message);
        Task NotifyAsync(string message);
        Task AnnounceAsync(string message);
        Task<Message> GetMessage(string messageId);
        Task<List<Message>> GetMessagesAsync(string topic,string serviceId,string typeId,int pageIndex,int pageSize,out int pageCount);
        Task<List<Message>> GetUserMessagesAsync(string userid, string serviceId,string typeId ,int pageIndex, int pageSize, out int pageCount);
        Task<bool> AddUserMessage(string userId,string messageId,string serviceId);
    }

}
