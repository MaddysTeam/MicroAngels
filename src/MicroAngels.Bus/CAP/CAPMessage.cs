using System;
using System.Data.SqlClient;

namespace MicroAngels.Bus.CAP
{

	public class CAPMessage : IMessage
	{

		public CAPMessage(string topic,string body,bool hasTrans)
		{
			Topic = topic;
			Body = body;
			HasTrans = hasTrans;
		}

		public string Topic { get; private set; }

		public string Body { get; private set; }

		public bool HasTrans { get; private set; }

	}

}
