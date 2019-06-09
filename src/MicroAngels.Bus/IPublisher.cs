using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Bus
{

	public interface IPublisher<Message> where Message:IMessage
	{
		Task<Message> PublishAsync(Message messge);
	}

}
