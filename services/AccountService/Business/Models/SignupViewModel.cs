using MicroAngels.Bus;

namespace Business
{

	public class SignupViewModel
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}

	public class CreateUserMessage: IMessage
	{
		public string UserName { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public string Topic { get; set; }
		public string Body { get; set; }
		public bool HasTrans { get; set; }
	}

}
