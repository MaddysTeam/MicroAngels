﻿using MicroAngels.Core;
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
			var validteReuslt = RoleAssets.Validate(roleAccess);
			if (validteReuslt.All(x => x.IsSuccess))
			{
				var effectCount = await RoleAssetsDb.AsInsertable(roleAccess).ExecuteCommandAsync();

				return effectCount > 0;
			}

			return false;
		}

		public async Task<bool> Edit(SystemRole role)
		{
			var validateResults = SystemRole.Validate(role);
			if (validateResults.All(r => r.IsSuccess))
			{
				if (role.RoleId.IsEmpty())
				{
					var current = RoleDb.GetSingle(r => r.RoleName == role.RoleName);
					return current.IsNull() ? await RoleDb.AsInsertable(role).ExecuteCommandAsync() > 0 : false;
				}
				else
				{
					return await RoleDb.AsUpdateable(role).ExecuteCommandAsync() > 0;
				}
			}

			return false;
		}

		public async Task<SystemRole> GetById(Guid id)
		{
			return await RoleDb.AsQueryable().FirstAsync(r => r.RoleId == id);
		}

		public async Task<List<SystemRole>> GetByUserIds(Guid[] userIds)
		{
			var query = DB
					.Queryable<SystemRole, UserRole, UserInfo>((r, ur, u) => new object[]
						 {
						JoinType.Inner,
						 r.RoleId == ur.Id,
						 ur.UserId==u.UserId
						 }).In((r, ur, u) => u.UserId, userIds);

			var result = await query.Select((r, ur, u) => r).ToListAsync();
			return result;
		}

		public async Task<List<SystemRole>> GetByUserName(string userName)
		{
			var query = DB
				.Queryable<SystemRole, UserRole, UserInfo>((r, ur, u) => new object[]
					 {
						 JoinType.Inner,
						 r.RoleId == ur.RoleId,
						 JoinType.Inner,
						 ur.UserId == u.UserId
					 }).Where((r, ur, u) => u.UserName == userName);

			var result = await query.Select((r, ur, u) => r).ToListAsync();

			return result;
		}

		public async Task<IEnumerable<SystemRole>> Search(Expression<Func<SystemRole, bool>> whereExpressions, int? pageSize, int? pageIndex)
		{
			var query = RoleDb.AsQueryable().Where(whereExpressions);
			if (pageSize.HasValue && pageIndex.HasValue)
				return await query.ToPageListAsync(pageIndex.Value, pageSize.Value);
			else
				return await query.ToListAsync();
		}

		public Task<bool> UnbindResource(Guid roleId, Guid assetsId)
		{
			var result = RoleAssetsDb.Delete(ra => ra.RoleId == roleId & ra.AssetId == assetsId);
			return Task.FromResult(result);
		}

	}

}
