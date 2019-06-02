using MessagePack;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileService.Business
{
	[MessagePackObject]
	public class Files
	{

		[Key(0)]
		[SugarColumn(IsPrimaryKey =true)]
		public Guid FildId { get; set; }

		public Guid FileType { get; set; }

		public Guid FileSource { get; set; }

		public string FileName { get; set; }

		public string FilePath { get; set; }

		public string MD5 { get; set; }

		public double FileSize { get; set; }

		public string FileExtension { get; set; }

	}

}
