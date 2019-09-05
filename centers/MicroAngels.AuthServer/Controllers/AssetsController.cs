using Business;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
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
			var assetViewMode = HierarchyMapFromAssets(assetsList, Root);

			return new JsonResult(new
			{
				data = assetViewMode
			});

		}

		[HttpPost("interfaces")]
		public IActionResult GetInterfaces([FromForm]int start, [FromForm]int length)
		{
			var page = new PageOptions(start, length);
			var interfacesResults = _service.SearchInterface(null, page);
			if (!interfacesResults.IsNull() && interfacesResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = interfacesResults.Select(x => x.Map<Interface, InterfaceViewModel>()),
					recordsTotal = page.TotalCount,
					recordsFiltered = page.TotalCount,
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
			var page = new PageOptions(start, length);
			var searchResults = _service.SearchMenu(null, page);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x => x.Map<Menu, MenuViewModel>()),
					recordsTotal = page.TotalCount,
					recordsFiltered = page.TotalCount,
				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}


		[HttpPost("hierarchyMenus")]
		public async Task<IActionResult> GetHierarchyMenus()
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var menus = await _service.GetMenusByUserId(userId.ToGuid());
			var assetsList = await _service.SearchAssets(asset => asset.AssetsType == Keys.Assests.MenuType);
			//bind link url
			if (menus.Count() <= 0)
			{
				return new JsonResult(new
				{
					data = Root
				});

			}
			foreach (var assets in assetsList)
			{
				var menu = menus.FirstOrDefault(m => m.MenuId == assets.ItemId);
				assets.Menu = menu;
			}

			//get hierachy asset view model
			var assetViewMode = HierarchyMapFromAssets(assetsList, Root);

			return new JsonResult(new
			{
				data = assetViewMode
			});
		}

		[HttpPost("menuInfo")]
		public async Task<IActionResult> GetMenuInfo([FromForm] Guid menuId)
		{
			var menu = await _service.GetMenuById(menuId);

			return new JsonResult(new
			{
				data = menu.Map<Menu, MenuViewModel>()
			});
		}

		[HttpPost("interfaceInfo")]
		public async Task<IActionResult> GetInterfaceInfo([FromForm] Guid interfaceId)
		{
			var inter = await _service.GetInterfaceById(interfaceId);

			return new JsonResult(new
			{
				data = inter.Map<Interface, InterfaceViewModel>()
			});
		}

		[HttpPost("editMenu")]
		public async Task<IActionResult> EditMenu([FromForm] MenuViewModel menuViewModel)
		{
			var isSuccess = await _service.EditMenu(
				 menuViewModel.Map<MenuViewModel, Menu>()
				 );

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("editInterface")]
		public async Task<IActionResult> EditInterface([FromForm] InterfaceViewModel interViewModel)
		{
			var isSuccess = await _service.EditInterface(
				interViewModel.Map<InterfaceViewModel, Interface>()
				);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("editList")]
		public async Task<IActionResult> EditAssetList([FromForm]  List<AssetsViewModel> list)
		{
			var assetsList = new List<Assets>();
			assetsList = HierarchyMapToAssets(assetsList, list[0]);

			var isSuccess = await _service.MultiEdit(assetsList);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}


		private readonly IAssetsService _service;

		private AssetsViewModel HierarchyMapFromAssets(IEnumerable<Assets> assetList, AssetsViewModel assetViewModel)
		{
			if (assetViewModel.IsNull())
				assetViewModel = Root;

			var parentId = assetViewModel.id;
			var children = assetList.Where(x => x.ParentId == parentId);
			foreach (var item in children)
			{
				var viewModel = item.Map<Assets, AssetsViewModel>();
				viewModel.children = new List<AssetsViewModel>();
				assetViewModel.children.Add(viewModel);

				HierarchyMapFromAssets(assetList, viewModel);
			}

			return assetViewModel;

		}

		private List<Assets> HierarchyMapToAssets(List<Assets> assetList, AssetsViewModel assetViewModel)
		{
			if (assetViewModel.IsNull() || assetList.IsNull())
				return assetList;

			Assets parent = assetViewModel.Map<AssetsViewModel, Assets>();
			if (!assetList.Exists(x => x.AssetsId == parent.AssetsId))
				assetList.Add(parent);

			var children = assetViewModel.children ?? new List<AssetsViewModel>();
			children.ForEach(v =>
			{
				var child = v.Map<AssetsViewModel, Assets>();
				child.ParentId = parent.AssetsId;
				assetList.Add(child);

				HierarchyMapToAssets(assetList, v);
			});

			return assetList;
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
