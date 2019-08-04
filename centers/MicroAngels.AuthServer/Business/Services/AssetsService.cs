using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroAngels.Core;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Business
{

	public class AssetsService : MySqlDbContext, IAssetsService
	{

		public AssetsService()
		{
		}

		public async Task<Assets> GetById(Guid assetId)
		{
			return await AssetsDb.AsQueryable().FirstAsync(asset => asset.AssetsId == assetId);
		}

		public async Task<bool> Edit(Assets assets)
		{
			if (Assets.Validate(assets).All(validateResult => validateResult.IsSuccess))
			{
				if (assets.AssetsId.IsEmpty())
				{
					var current = AssetsDb.GetSingle(ass => ass.AssetsName == assets.AssetsName);
					return current.IsNull() ? await AssetsDb.AsInsertable(assets).ExecuteCommandAsync() > 0 : false;
				}
				else
				{
					return await AssetsDb.AsUpdateable(assets).ExecuteCommandAsync() > 0;
				}
			}

			return false;
		}

		public async Task<bool> EditInterface(Interface inter)
		{
			var result = false;
			if (Interface.Validate(inter).All(validateResult => validateResult.IsSuccess))
			{
				result = DB.UseTranAsync(async () =>
				{
					await DB.Insertable(inter).ExecuteCommandAsync();
					await DB.Insertable(new Assets
					{
						AssetsName = inter.Title,
						AssetsStatus = Keys.EnableStatus,
						AssetsType = Keys.Assests.MenuType,
						ItemId = inter.InterfaceId,
						SystemId = Keys.System.DefaultSystemId
					}).ExecuteCommandAsync();
				}).IsCompletedSuccessfully;
			}
			else
			{
				var assets = await SearchAssets(ass => ass.ItemId == inter.InterfaceId);
				if (assets.Count() > 0)
				{
					var asset = assets.FirstOrDefault();
					asset.AssetsName = inter.Title;
					result = await AssetsDb.AsUpdateable(asset).ExecuteCommandAsync() > 0;
				}

				result = await InterfaceDb.AsUpdateable(inter).ExecuteCommandAsync() > 0;
			}

			return false;
		}

		public async Task<bool> EditMenu(Menu menu)
		{
			var result = false;
			if (Menu.Validate(menu).All(validateResult => validateResult.IsSuccess))
			{
				if (menu.MenuId.IsEmpty() && MenuDb.GetSingle(m => m.Title == menu.Title).IsNull())
				{
					menu.MenuId = Guid.NewGuid();
					result = DB.UseTranAsync(async () =>
					{
						await DB.Insertable(menu).ExecuteCommandAsync();
						await DB.Insertable(new Assets
						{
							AssetsName = menu.Title,
							AssetsStatus = Keys.EnableStatus,
							AssetsType = Keys.Assests.MenuType,
							ItemId = menu.MenuId,
							SystemId = menu.SystemId
						}).ExecuteCommandAsync();
					}).IsCompletedSuccessfully;

				}
				else
				{
					var assets = await SearchAssets(ass => ass.ItemId == menu.MenuId);
					if (assets.Count() > 0)
					{
						var asset = assets.FirstOrDefault();
						asset.AssetsName = menu.Title;
						result = await AssetsDb.AsUpdateable(asset).ExecuteCommandAsync() > 0;
					}

					result = await MenuDb.AsUpdateable(menu).ExecuteCommandAsync() > 0;
				}
			}

			return result;
		}

		public async Task<bool> BindAssets(Guid assetsId, Guid itemId)
		{
			if (assetsId.IsEmpty() || itemId.IsEmpty())
			{
				return false;
			}

			return await DB.Updateable<Assets>(a => a.AssetsId == assetsId)
								 .UpdateColumns(a => new { ItemId = itemId })
								 .ExecuteCommandAsync() > 0;
		}

		public async Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames)
		{
			if (roleNames.IsNull())
			{
				return null;
			}

			var query = DB.Queryable<SystemRole, RoleAssets, Assets, Interface>((r, ra, a, i) =>
					new object[]{
						JoinType.Inner,
						r.RoleId==ra.RoleId,
						JoinType.Inner,
						ra.AssetId==a.AssetsId,
						JoinType.Inner,
						a.ItemId==i.InterfaceId
					}).Where((r, ra, a, i) => i.IsAllowAnonymous || roleNames.Contains(r.RoleName));


			var result = await query.Select((r, ra, a, i) => i).ToListAsync();

			return result;
		}

		public async Task<IEnumerable<Menu>> GetMenusByRoleNames(string[] roleNames)
		{
			if (roleNames.IsNull())
			{
				return null;
			}

			var query = DB.Queryable<SystemRole, RoleAssets, Assets, Menu>((r, ra, a, m) =>
					new object[]{
						JoinType.Inner,
						r.RoleId==ra.RoleId,
						JoinType.Inner,
						ra.AssetId==a.AssetsId,
						JoinType.Inner,
						a.ItemId== m.MenuId
					}).Where((r, ra, a, i) => roleNames.Contains(r.RoleName));


			var result = await query.Select((r, ra, a, m) => m).ToListAsync();

			return result;
		}

		public async Task<IEnumerable<Menu>> GetMenusByUserId(Guid userId)
		{
			if (!userId.IsEmpty())
			{
				return null;
			}

			var query = DB.Queryable<UserInfo, SystemRole, RoleAssets, Assets, Menu>((u, r, ra, a, m) =>
				 new object[]{
						JoinType.Inner,
						u.UserId==r.RoleId,
						JoinType.Inner,
						r.RoleId==ra.RoleId,
						JoinType.Inner,
						ra.AssetId==a.AssetsId,
						JoinType.Inner,
						a.ItemId== m.MenuId
				 }).Where((u, r, ra, a, m) => u.UserId == userId);

			var result = await query.Select((u, r, ra, a, m) => m).ToListAsync();
			return result;
		}

		public async Task<IEnumerable<Assets>> GetRoleAssets(Guid roleId)
		{
			var query = DB.Queryable<Assets, RoleAssets>((a, ra) =>
					  new object[]{
						JoinType.Left,
						a.AssetsId==ra.AssetId && ra.RoleId==roleId,
					  });

			var result = await query.Select((a, ra) =>
			new Assets
			{
				AssetsId = a.AssetsId,
				AssetsName = a.AssetsName,
				AssetsStatus = a.AssetsStatus,
				AssetsType = a.AssetsType,
				Description = a.Description,
				IsBind = ra.Id != Guid.Empty,
				ItemId = a.ItemId,
				ParentId = a.ParentId,
				SystemId = a.SystemId
			}).ToListAsync();

			return result;
		}

		public IEnumerable<Interface> SearchInterface(Expression<Func<Interface, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount)
		{
			totalCount = 0;
			var query = whereExpressions == null ? InterfaceDb.AsQueryable() : InterfaceDb.AsQueryable().Where(whereExpressions);

			if (pageSize.HasValue && pageIndex.HasValue)
			{
				return query.ToPageList(pageIndex.Value, pageSize.Value, ref totalCount);
			}
			else
				return query.ToList();
		}

		public IEnumerable<Menu> SearchMenu(Expression<Func<Menu, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount)
		{
			totalCount = 0;
			var query = whereExpressions == null ? MenuDb.AsQueryable() : MenuDb.AsQueryable().Where(whereExpressions);

			if (pageSize.HasValue && pageIndex.HasValue)
			{
				return query.ToPageList(pageIndex.Value, pageSize.Value, ref totalCount);
			}
			else
				return query.ToList();
		}

		public async Task<IEnumerable<Assets>> SearchAssets(Expression<Func<Assets, bool>> whereExpressions)
		{
			var query = whereExpressions == null ? AssetsDb.AsQueryable() : AssetsDb.AsQueryable().Where(whereExpressions);

			return await query.ToListAsync();
		}

		public async Task<Menu> GetMenuById(Guid menuId)
		{
			return await MenuDb.AsQueryable().FirstAsync(menu => menu.MenuId == menuId);
		}

		public async Task<bool> MultiEdit(List<Assets> assetsList)
		{
			var result = await DB.UseTranAsync(async () =>
			{
				foreach (var assets in assetsList)
				{
					//if (Assets.Validate(assets).All(validateResult => validateResult.IsSuccess))
					 AssetsDb.Update(a=>new Assets{ ParentId= assets.ParentId }, it => it.AssetsId == assets.AssetsId);
				}
			});

			return result.IsSuccess;
		}
	}

}
