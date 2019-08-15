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

}
