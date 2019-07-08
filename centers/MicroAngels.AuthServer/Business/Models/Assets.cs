using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	/// <summary>
	/// 资产，资源
	/// </summary>
	public class Assets
	{
		[SugarColumn(IsPrimaryKey = true,Length =50)]
		public Guid AssetsId { get; set; }
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid SystemId { get; set; }
		public string AssetsName { get; set; }
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid AssetsType { get; set; }
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid AssetsStatus { get; set; }
		[SugarColumn(IsNullable = true, Length = 3000)]
		public string Description { get; set; }
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid ParentId { get; set; }
		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid ItemId { get; set; }

		public static List<ValidateResult> Validate(Assets assets)
		{
			return assets
				.NotNullOrEmpty(assets.AssetsName, "")
				.NotNull(assets.SystemId,"")
				.Validate();
		}
	}

	public enum AssetsStatus
	{
		enable,
		disable
	}

	/// <summary>
	/// 资产-菜单
	/// </summary>
	public class Menu
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid MenuId { get; set; }
		public string Title { get; set; }
	}

	/// <summary>
	/// 资产-按钮
	/// </summary>
	public class Button
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid ButtonId { get; set; }
		public string Title { get; set; }
	}

	/// <summary>
	/// 资产- 接口
	/// </summary>
	public class Interface
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid InterfaceId { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public string Parmas { get; set; }
		public string Version { get; set; }
		public string Method { get; set; }

		public static string[] Methods = { "GET", "POST", "PUT", "DELETE" };

		public static List<ValidateResult> Validate(Interface iinterface)
		{
			return
			iinterface.NotNull(iinterface.Title, "")
			 .NotNull(iinterface.Url, "")
			 .RegexIsMatch(iinterface.Url,RegexSupporter.UrlPatterns,"")
			 .IsIn(Methods,iinterface.Method)
			 .Validate();
		}

	}


}
