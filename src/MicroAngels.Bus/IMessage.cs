using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.Bus
{

	public interface IMessage
	{
		string Topic { get; }
		string Body { get; }
	}

}
