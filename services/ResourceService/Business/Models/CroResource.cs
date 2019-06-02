using SqlSugar;
using System;
using MicroAngels.Core.Plugins;
using System.Collections.Generic;

namespace ResourceService.Business
{

	public class CroResource
	{

		[SugarColumn(IsPrimaryKey = true)]
		public Guid CrosourceId { get; set; }

		[SugarColumn(IsNullable =false)]
		public Guid FildId { get; set; }

		[SugarColumn(IsNullable = false)]
		public Guid MediumTypeId { get; set; }

		[SugarColumn(IsNullable = false, Length = 100)]
		public string Title { get; set; }

		[SugarColumn(Length = 100)]
		public string Author { get; set; }

		[SugarColumn(Length = 500)]
		public string Keywords { get; set; }

		public int DownCount { get; set; }

		public int FavoriteCount { get; set; }

		public int CommentCount { get; set; }

		public int StarCount { get; set; }

		public Guid Creator { get; set; }

		public static List<ValidateResult> Validate(CroResource r)
		{
			return
			r.NotNull(r.CrosourceId, Keys.Errors.NOT_ALLOWED_ID_NULL)
			 .NotNull(r.FildId,"")
			 .Length(r.Author, 100, "")
			 .Length(r.Title, 100, "")
			 .Length(r.Keywords,500,"")
			 .Validate();
		}

	}

}
