using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Bus
{

	public interface IPublisher
	{
		Task<Message> PublishAsync<Message>(Message messge) where Message:IMessage;
	}

}
