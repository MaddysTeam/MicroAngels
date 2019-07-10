using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroAngels.Core;
using System;
using System.Linq;

namespace Business
{

	public class AssetsService : MySqlDbContext, IAssetsService
	{

		public AssetsService()
		{
		}

		public async Task<bool> Edit(Assets assets)
		{
			if (Assets.Validate(assets).All(validateResult => validateResult.IsSuccess))
			{
				if (assets.AssetsId.IsEmpty())
				{
					return await AssetsDb.AsInsertable(assets).ExecuteCommandAsync() > 0;
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
				return await InterfaceDb.AsInsertable(iinterface).ExecuteCommandAsync() > 0;
			}
			else
			{
				return await InterfaceDb.AsUpdateable(iinterface).ExecuteCommandAsync() > 0;
			}

		}

		public async Task<bool> BindInterface(Guid assetsId, Guid interfaceId)
		{
			if (assetsId.IsEmpty() || interfaceId.IsEmpty())
			{
				return false;
			}

			return await DB.Updateable<Assets>(a => a.AssetsId == assetsId)
								 .UpdateColumns(a => new { ItemId = interfaceId })
								 .ExecuteCommandAsync() > 0;
		}

		public async Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames)
		{
			if(roleNames.IsNull() || roleNames.Length == 0)
			{
				return null;
			}

			var query = DB.Queryable< SystemRole, RoleAssets, Assets, Interface>((r, ra, a, i) =>
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

		public async Task<Assets> GetById(Guid assetId)
		{
			return await AssetsDb.AsQueryable().FirstAsync(asset => asset.AssetsId == assetId);
		}
	}

}
