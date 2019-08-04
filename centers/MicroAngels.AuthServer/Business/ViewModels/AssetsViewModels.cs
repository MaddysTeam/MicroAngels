using System;
using System.Collections.Generic;

namespace Business
{

	public class MenuViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string LinkUrl { get; set; }
		public Guid SystemId { get; set; }
	}

	public class InterfaceViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Params { get; set; }
		public string Version { get; set; }
		public string Method { get; set; }
		public string Url { get; set; }
		public bool IsAllowAnonymous { get; set; }
	}

	public class AssetsViewModel
	{
		public string title { get; set; }
		public Guid id { get; set; }
		public Guid parentId { get; set; }
		public bool isbind { get; set; }
		public List<AssetsViewModel> children { get; set; }

		public string link { get; set; }
	}

}
