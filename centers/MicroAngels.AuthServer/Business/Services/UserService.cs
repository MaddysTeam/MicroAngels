using MicroAngels.Cache.Redis;
using MicroAngels.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public class UserService : MySqlDbContext, IUserService
	{

		public async Task<bool> Edit(UserInfo userInfo)
		{
			if (UserInfo.Validate(userInfo).All(u => u.IsSuccess))
			{
				if (userInfo.UserId.IsEmpty())
				{
					var current = UserDb.GetSingle(ur => ur.UserName == userInfo.UserName);
					if (current.IsNull())
					{
						userInfo.UserId = Guid.NewGuid();
						return await UserDb.AsInsertable(userInfo).ExecuteCommandAsync() > 0;
					}
					else
						return false;
				}
				else
					return await UserDb.AsUpdateable(userInfo).ExecuteCommandAsync() > 0;
			}

			return false;
		}

		public async Task<UserInfo> GetById(Guid id)
		{
			return UserDb.GetById(id);
		}

		public IEnumerable<UserInfo> Search(Expression<Func<UserInfo, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount)
		{
			totalCount = 0;
			var query = whereExpressions == null ? UserDb.AsQueryable() : UserDb.AsQueryable().Where(whereExpressions);

			if (pageSize.HasValue && pageIndex.HasValue)
			{
				return query.ToPageList(pageIndex.Value, pageSize.Value, ref totalCount);
			}
			else
				return query.ToList();
		}

		public async Task<bool> BindRoles(Guid userId, string[] roleIds)
		{
			bool result = false;

			await UserRoleDb.AsDeleteable().Where(ur => ur.UserId == userId).ExecuteCommandAsync();

			foreach (var roleId in roleIds)
			{
				var userRole = new UserRole() { Id = Guid.NewGuid(), RoleId = roleId.ToGuid(), UserId = userId };
				result = await UserRoleDb.AsInsertable(userRole).ExecuteCommandAsync() > 0;
			}

			return result;
		}

		public Task<bool> UnbindRole(Guid userRoleId)
		{
			var result = false;
			var userRole = UserRoleDb.GetById(userRoleId);
			if (!userRole.IsNull())
			{
				result = UserRoleDb.DeleteById(userRoleId);
			}

			return Task.FromResult(result);
		}


		public UserInfo GetByName(string name)
		{
			var user = UserDb.GetSingle(u => string.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase));
			return user;
		}

		//private readonly IRedisCache _cache;

	}

}
