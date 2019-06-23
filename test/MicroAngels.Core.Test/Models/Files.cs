using System;

namespace MicroAngels.Core.Test.Models
{

	public class Files
	{
		public Guid FildId { get; set; }
		public Guid FileType { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string MD5 { get; set; }
		public double FileSize { get; set; }
		public string FileExtension { get; set; }
	}

}
