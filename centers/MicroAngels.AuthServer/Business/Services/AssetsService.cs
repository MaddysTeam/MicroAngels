using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroAngels.Core;

namespace Business
{

	public class AssetsService : MySqlDbContext, IAssetsService
	{

		public AssetsService()
		{
		}

		public async Task<bool> EditAsset(Assets assets)
		{
			if (!assets.IsNull())
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

			if (iinterface.InterfaceId.IsEmpty())
			{
				return await InterfaceDb.AsInsertable(iinterface).ExecuteCommandAsync() > 0;
			}
			else
			{
				return await InterfaceDb.AsUpdateable(iinterface).ExecuteCommandAsync() > 0;
			}

		}

		public async Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames)
		{
			var query = DB.Queryable<Interface, RoleAssets, SystemRole, Assets>((i, ra, r, a) =>
					 new object[]{
						JoinType.Inner,
						r.RoleId==ra.RoleId,
						ra.AssetId==a.AssetsId,
						a.ItemId==i.InterfaceId
					 }).In((i, ra, r, a) => r.RoleName, roleNames);

			var result = await query.Select((i, ra, r, a) => i).ToListAsync();

			return result;
		}

	}

}
