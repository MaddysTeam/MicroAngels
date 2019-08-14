using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

	public class TopicViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string ServiceId { get; set; }
		public string Description { get; set; }
		public DateTime CreateTime { get; set; }
	}

}
