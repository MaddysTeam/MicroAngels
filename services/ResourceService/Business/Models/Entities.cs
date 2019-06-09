using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceService.Business
{

	/// <summary>
	/// 评价资源
	/// </summary>
	public class CroResourcecommend
	{
		public Guid CrosourceId { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string CoverPath { get; set; }
		public int StarCount { get; set; }
		public int StarTotal { get; set; }
		public int Star { get { if (StarCount == 0) return 0; return StarTotal / StarCount; } }
		//public string FitCoverPath
		//{
		//	get
		//	{
		//		if (CoverPath == "")
		//			return "/assets/img/cover.png";
		//		return CoverPath;
		//	}
		//}
	}


	//我的资源

	public class CroMyResource
	{
		public Guid CrosourceId { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string CoverPath { get; set; }
		public string FileExtName { get; set; }
		public string Description { get; set; }
		public DateTime OccurTime { get; set; }
		public long StatePKID { get; set; }

		public Guid OccurId { get; set; }
		public string Content { get; set; }

		//public long Audittype { get; set; }
		public Guid Auditor { get; set; }
		public DateTime AuditedTime { get; set; }
		public string AuditOpinion { get; set; }
		public string FitCoverPath
		{
			get
			{
				if (CoverPath == "")
					return "/assets/img/cover.png";
				return CoverPath;
			}
		}
	}

}
