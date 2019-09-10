using Business.Helpers;
using MicroAngels.Bus;
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
		public bool IsFriend { get; set; }
	}

	public class FriendViewModel
	{
		public string SubscriberId { get; set; }
		public string TopicId { get; set; }
		public string TargetId { get; set; }
		public string ServiceId { get; set; }
	}

	public class UserSearchOption: BaseViewModel
	{

	}

}
