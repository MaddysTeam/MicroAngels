﻿using SqlSugar;
using System;

namespace Business
{

	/// <summary>
	/// 资产，资源
	/// </summary>
	public class Assets
	{
		[SugarColumn(IsPrimaryKey = true)]
		public Guid AssetsId { get; set; }
		public Guid SystemId { get; set; }
		public string AssetsName { get; set; }
		public Guid AssetsType { get; set; }
		public Guid AssetsStatus { get; set; }
		public string Description { get; set; }
		public Guid ParentId { get; set; }
		public Guid ItemId { get; set; }
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
		public string[] Parmas { get; set; }
		public string Version { get; set; }
	}


}