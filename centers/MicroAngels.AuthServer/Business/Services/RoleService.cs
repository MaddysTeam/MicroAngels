using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public class RoleService : MySqlDbContext, IRoleService
	{

		public RoleService()
		{
		}

		public async Task<bool> BindResource(RoleAssets roleAccess)
		{
			var effectCount = await RoleAssetsDb.AsInsertable(roleAccess).ExecuteCommandAsync();

			return effectCount > 0;
		}

		public async Task<bool> Edit(SystemRole role)
		{
			var validateResults = SystemRole.Validate(role);
			if (validateResults.All(r => r.IsSuccess))
			{
				var effectCount = await RoleDb.AsUpdateable(role).ExecuteCommandAsync();

				return effectCount > 0;
			}

			return false;
		}

		public async Task<SystemRole> GetById(Guid id)
		{
			return await RoleDb.AsQueryable().FirstAsync(r => r.RoleId == id);
		}

		public async Task<List<SystemRole>> GetByUserName(string userName)
		{
			var query = DB
				.Queryable<SystemRole, UserRole, UserInfo>((r, ur, u) => new object[]
					 {
						JoinType.Inner,
						 r.RoleId == ur.Id,
						 ur.UserId==u.UserId
					 }).Where((r,ur,u)=> u.UserName==userName);

			var result = await query.Select((r, ur, u) => r).ToListAsync();

			return result;
		}

		public Task<IEnumerable<SystemRole>> Search(List<Expression<Func<SystemRole, bool>>> whereExpressions, int? pageSize, int? pageIndex)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UnbindResource(Guid roleId, Guid assetsId)
		{
			throw new NotImplementedException();
		}

	}

}
