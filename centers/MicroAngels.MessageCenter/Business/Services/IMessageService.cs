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
		Task<List<UserMessage>> GetUnReadMessage(MessageSearchOptions options);
		Task<List<Message>> Search(MessageSearchOptions options, PageOptions pageViewModel);
        Task<List<UserMessage>> SearchUserMessage(MessageSearchOptions options, PageOptions pageViewModel);
		Task<bool> ReceiveMessages(MessageSearchOptions options);
    }

}
