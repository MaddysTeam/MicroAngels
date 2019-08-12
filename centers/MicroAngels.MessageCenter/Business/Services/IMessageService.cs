using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{

	public interface IMessageService
    {
        Task<bool> SubscribeAsync(string message);
        Task<bool> NotifyAsync(string message);
        Task<bool> AnnounceAsync(string message);
		Task<bool> AnnounceAsync(Message message);
		Task<Message> GetMessage(string messageId);
        Task<List<Message>> GetMessagesAsync(string topic,string serviceId,string typeId,int pageIndex,int pageSize,out int pageCount);
        Task<List<UserMessage>> GetUserMessagesAsync(string userid, string serviceId, string topicId, string typeId, int pageIndex, int pageSize, out int pageCount);
    }

}
