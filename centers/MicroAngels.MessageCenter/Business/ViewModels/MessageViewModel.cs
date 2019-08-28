using System;

namespace Business
{

	public class MessageViewModel
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public string TopicId { get; set; }
		public string Topic { get; set; }
		public string ServiceId { get; set; }
		public DateTime SendTime { get; set; }
	}

	public class MessageSearchOptions
	{
		public string topicId { get; set; }
		public string serviceId { get; set; }
		public string[] senderIds { get; set; }
		public string reveiverId { get; set; }
		public string typeId { get; set; }
		public string statusId { get; set; }
	}


	public class SubscribeSearchOptions
	{
		public string topicId { get; set; }
		public string serviceId { get; set; }
		public string subscriberId { get; set; }
		public string targetId { get; set; }
		public string code { get; set; }
	}


	public class UserMessageViewModel
	{
		public string Id { get; set; }
		public string ServiceId { get; set; }
		public string MessageId { get; set; }
		public string ReceiverId { get; set; }
		public string StatusId { get; set; }
		public string Body { get; set; }
	}


	public class SubscribeViewModel
	{
		public string Id { get; set; }
		public string SubscriberId { get; set; }
		public string TopicId { get; set; }
		public string TargetId { get; set; }
		public string ServiceId { get; set; }
		public string Target { get; set; }
		public string Subsriber { get; set; }
	}

}
