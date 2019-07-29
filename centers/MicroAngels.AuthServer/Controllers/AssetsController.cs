using Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AssetsController : ControllerBase
	{

		public AssetsController(IAssetsService service)
		{
			_service = service;
		}

		[HttpPost("urls")]
		public async Task<string[]> GetUrls([FromBody]string[] roles)
		{
			var interfaces = await _service.GetInterfaceByRoleNames(roles);

			return !interfaces.IsNull() && interfaces.Count() > 0 ? interfaces.Select(x => x.Url).ToArray() : null;
		}

		[HttpPost("roleAssets")]
		public async Task<IActionResult> GetAssets([FromForm]Guid roleId)
		{
			var assetsList = await _service.GetRoleAssets(roleId);
			var assetViewMode = HierarchyAssets(assetsList, Root);

			return new JsonResult(new {
				data = assetViewMode
			});

		}

		[HttpPost("interfaces")]
		public IActionResult GetInterfaces([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var interfacesResults = _service.SearchInterface(null, length, start, out totalCount);

			if (!interfacesResults.IsNull() && interfacesResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = interfacesResults.Select(x => new
					{
						title = x.Title,
						url = x.Url,
						method = x.Method,
						param = x.Parmas,
						IsAnonymous = x.IsAllowAnonymous
					}),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,
				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}

		[HttpPost("allMenus")]
		public IActionResult GetAllMenus([FromForm]int start, [FromForm]int length)
		{
			var totalCount = 0;
			var searchResults = _service.SearchMenu(null, length, start, out totalCount);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x => new
					{
						id = x.MenuId,
						title = x.Title,
						systemId = x.SystemId,
						url = x.LinkUrl
					}),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,

				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}

		[HttpPost("menus")]
		public async Task<List<Menu>> GetMenus([FromForm]Guid userId)
		{
			var menus = await _service.GetMenusByUserId(userId);

			return menus.ToList();
		}

		[HttpPost("menuInfo")]
		public async Task<IActionResult> GetMenuInfo([FromForm] Guid menuId)
		{
			var result = await _service.GetMenuById(menuId);

			return new JsonResult(new
			{
				data = new MenuViewModel { Id = result.MenuId, LinkUrl = result.LinkUrl, Title = result.Title }
			});
		}

		[HttpPost("editMenu")]
		public async Task<IActionResult> EditMenu([FromForm] MenuViewModel menu)
		{
			var isSuccess = await _service.EditMenu(
				new Menu
				{
					MenuId = menu.Id,
					LinkUrl = menu.LinkUrl,
					Title = menu.Title,
					SystemId = Keys.System.DefaultSystemId
				});

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("editInterface")]
		public async Task<IActionResult> EditInterface([FromForm] InterfaceViewModel inter)
		{
			var isSuccess = await _service.EditInterface(
				new Interface
				{
					InterfaceId = inter.Id,
					Method = inter.Method,
					IsAllowAnonymous = inter.IsAllowAnonymous,
					Url = inter.Url,
					Version = inter.Version,
					Parmas = inter.Params,
					Title = inter.Title
				});

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("editList")]
		public async Task<IActionResult> EditAssetList([FromForm] List<AssetsViewModel> list)
		{
			var isSuccess = true;

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}


		private readonly IAssetsService _service;


		private AssetsViewModel HierarchyAssets(IEnumerable<Assets> assetList, AssetsViewModel assetViewModel)
		{
			if (assetViewModel.IsNull())
				assetViewModel = Root;

			var parentId = assetViewModel.id;
			var children = assetList.Where(x => x.ParentId == parentId);
			foreach (var item in children)
			{
				var viewModel = new AssetsViewModel
				{
					id =item.AssetsId ,
					parentId = parentId,
					isbind = item.IsBind,
					title = item.AssetsName,
					children = new List<AssetsViewModel>()
				};

				assetViewModel.children.Add(viewModel);

				HierarchyAssets(children, viewModel);
			}

			return assetViewModel;

		}


		private AssetsViewModel Root => new AssetsViewModel
		{
			children = new List<AssetsViewModel>(),
			id = Guid.Empty,
			parentId = Guid.Empty,
			isbind = false,
			title = "资源"
		};

	}

}
