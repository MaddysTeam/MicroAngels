using System;

namespace Business
{

	public class UserViewModel
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string HeaderImagePath { get; set; }
	}

	public class FriendViewModel
	{
		public string Id { get; set; }
		public string SubscriberId { get; set; }
		public string TopicId { get; set; }
		public string TargetId { get; set; }
		public string ServiceId { get; set; }
	}

}
