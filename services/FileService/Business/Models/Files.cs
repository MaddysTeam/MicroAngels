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
		[SugarColumn(IsPrimaryKey = true,Length = 50)]
		public Guid FildId { get; set; }

		[Key(1)]
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid FileType { get; set; }

		[Key(2)]
		[SugarColumn(IsNullable = false)]
		public string FileName { get; set; }

		[Key(3)]
		[SugarColumn(IsNullable = false)]
		public string FilePath { get; set; }

		[SugarColumn(IsNullable = false)]
		public string MD5 { get; set; }

		[SugarColumn(IsNullable = false,DecimalDigits =0)]
		public double FileSize { get; set; }

		[SugarColumn(IsNullable = false)]
		public string FileExtension { get; set; }

	}

}
