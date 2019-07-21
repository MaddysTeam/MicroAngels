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
		//public UserService(IRedisCache cache)
		//{
		//	_cache = cache;
		//}

		public async Task<bool> Edit(UserInfo userInfo)
		{
			if (UserInfo.Validate(userInfo).All(u => u.IsSuccess))
			{
				if (userInfo.UserId.IsEmpty())
				{
					var current = UserDb.GetSingle(ur => ur.UserName == userInfo.UserName);
					return current.IsNull() ? await UserDb.AsInsertable(userInfo).ExecuteCommandAsync() > 0 : false;
				}
				else
					return await UserDb.AsUpdateable(userInfo).ExecuteCommandAsync() > 0;

				//_cache.AddOrRemove(userInfo.UserId,userInfo,TimeSpan.MaxValue);
			}


			return false;
		}

		public Task<UserInfo> GetById(Guid id)
		{
			return Task.FromResult(UserDb.GetById(id));
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

		public async Task<bool> BindRole(UserRole userRole)
		{
			bool result = false;
			var validteReuslt = UserRole.Validate(userRole);
			if (validteReuslt.All(x => x.IsSuccess))
			{
				var current = UserRoleDb.GetSingle(ur => ur.UserId == userRole.UserId & ur.RoleId == userRole.RoleId);
				if (current.IsNull())
				{
					result = await UserRoleDb.AsInsertable(userRole).ExecuteCommandAsync() > 0;
				}
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

		//public async Task<IEnumerable<UserRole>> SearchUserRole(Guid userId, Guid roleId)
		//{
		//	return await UserRoleDb.AsQueryable().Where(ur => ur.UserId == userId & ur.RoleId == roleId).WithCache().ToListAsync();
		//}

		public UserInfo GetByName(string name)
		{
			var user = UserDb.GetSingle(u => string.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase));
			return user;
		}

		private readonly IRedisCache _cache;

	}

}
