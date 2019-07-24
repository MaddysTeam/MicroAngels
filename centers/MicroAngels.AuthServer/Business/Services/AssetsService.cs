﻿using SqlSugar;
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

		public Task<IEnumerable<Assets>> GetAssets(Expression<Func<Assets, bool>> whereExpressions)
		{
			throw new NotImplementedException();
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

		public async Task<bool> EditInterface(Interface iinterface)
		{
			if (Interface.Validate(iinterface).All(validateResult => validateResult.IsSuccess))
			{
				if (iinterface.InterfaceId.IsEmpty())
				{
					var current = InterfaceDb.GetSingle(inter => inter.Title == iinterface.Title);
					return current.IsNull() ? await InterfaceDb.AsInsertable(iinterface).ExecuteCommandAsync() > 0 : false;
				}
				else
				{
					return await InterfaceDb.AsUpdateable(iinterface).ExecuteCommandAsync() > 0;
				}
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
					var assets = await GetAssets(ass => ass.ItemId == menu.MenuId);
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

		public IEnumerable<Interface> GetInterface(Expression<Func<Interface, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount)
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

	}

}
