using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

	public class CroResource
	{

		public long CrosourceId { get; set; }

		public long FildId { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }

		public string Keywords { get; set; }

		public long MediumTypeId { get; set; }

		public long DownCount { get; set; }

		public long FavoriteCount { get; set; }

		public long CommentCount { get; set; }

		public long StarCount { get; set; }
	}

}
