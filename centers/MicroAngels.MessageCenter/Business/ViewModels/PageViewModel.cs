using MicroAngels.Core;

namespace Business
{

	public class PageOptions
	{
		public PageOptions(int startIndex, int pageSize)
		{
			this.startIndex = startIndex;
			this.pageSize = pageSize;
		}

		int startIndex { get; set; }
		int pageSize { get; set; }

		int? pageIndex => pageSize <= 0 ? 0 : (startIndex / pageSize) + 1;

		public int PageIndex => pageIndex.Value;
		public int PageSize => pageSize;
		public int TotalCount { get; set; }
		public bool IsValidate => pageSize > 0;
	}

}
