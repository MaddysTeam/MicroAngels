using MicroAngels.Core.Plugins;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Business
{

	/// <summary>
	/// Assets for describe some resouce
	/// </summary>
	public class Assets
	{
		[SugarColumn(IsPrimaryKey = true,Length =50)]
		public Guid AssetsId { get; set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public Guid SystemId { get; set; }

		[SugarColumn(IsNullable = false, Length = 200)]
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

		[SugarColumn(IsIgnore =true)]
		public bool IsBind { get; set; }

		[SugarColumn(IsIgnore = true)]
		public Menu Menu { get; set; }

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
	/// assets menu
	/// </summary>
	public class Menu
	{
		[SugarColumn(IsPrimaryKey = true,Length =50)]
		public Guid MenuId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public Guid SystemId { get; set; }

		[SugarColumn(IsNullable = false, Length = 200)]
		public string Title { get; set; }

		[SugarColumn(IsNullable = true, Length = 2000)]
		public string LinkUrl { get; set; }

		public static List<ValidateResult> Validate(Menu menu)
		{
			return
			menu
			 .NotNullOrEmpty(menu.Title, "")
			 .Validate();
		}

	}

	/// <summary>
	/// assets butotn
	/// </summary>
	//public class Button
	//{
	//	[SugarColumn(IsPrimaryKey = true)]
	//	public Guid ButtonId { get; set; }
	//	public string Title { get; set; }
	//}

	/// <summary>
	/// assets interface
	/// </summary>
	public class Interface
	{
		[SugarColumn(IsPrimaryKey = true,Length =50)]
		public Guid InterfaceId { get; set; }

		[SugarColumn(IsNullable=false,Length = 100)]
		public string Title { get; set; }

		[SugarColumn(IsNullable = true, Length = 1000)]
		public string Url { get; set; }

		[SugarColumn(IsNullable = true, Length = 1000)]
		public string Parmas { get; set; }

		[SugarColumn(IsNullable = true, Length = 50)]
		public string Version { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string Method { get; set; }

		public bool IsAllowAnonymous { get; set; }

		public static string[] Methods = { "GET", "POST", "PUT", "DELETE" };

		public static List<ValidateResult> Validate(Interface iinterface)
		{
			return
			iinterface
			 .NotNullOrEmpty(iinterface.Title, "")
			 .NotNullOrEmpty(iinterface.Url, "")
			// .RegexIsMatch(iinterface.Url,RegexSupporter.UrlPatterns,"")
			 .IsIn(Methods,iinterface.Method)
			 .Validate();
		}

	}

	public static class AssetsKeys
	{
		public static Guid InterfaceType = Guid.Parse("94348b14-58a8-4156-b3bf-a5940a706932");
		public static Guid MenuType = Guid.Parse("b78fa535-4a34-419d-9aa4-63f56224d0ba");
		//public static Guid EnableStatus = Guid.Parse("");
		//public static Guid DisableStatus = Guid.Parse("");
		//public static Guid ReadonlyStatus = Guid.Parse("");
	}



}
