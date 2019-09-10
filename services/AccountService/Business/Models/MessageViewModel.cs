using Business.Helpers;
using MicroAngels.Bus;

namespace Business
{

	public class AddUserMessage : IMessage
	{
		public string UserName { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public string Topic { get; set; }
		public string Body { get; set; }
		public bool HasTrans { get; set; }
	}


	public class AddAccountMessage : IMessage
	{
		public string Topic => AppKeys.AddAccount;
		public string Body { get; set; }
		public bool HasTrans { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}

}
