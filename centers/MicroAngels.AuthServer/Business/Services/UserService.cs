using MicroAngels.Cache.Redis;
using MicroAngels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public class UserService : MySqlDbContext, IUserService
	{
		public UserService(IRedisCache cache)
		{
			_cache = cache;
		}

		public async Task<UserInfo> Edit(UserInfo userInfo)
		{
			var validateResults = UserInfo.Validate(userInfo);
			if (validateResults.All(u => u.IsSuccess))
			{
				if (userInfo.UserId.IsEmpty())
					await UserDb.AsInsertable(userInfo).ExecuteCommandAsync();
				else
					await UserDb.AsUpdateable(userInfo).ExecuteCommandAsync();

				//_cache.AddOrRemove(userInfo.UserId,userInfo,TimeSpan.MaxValue);
			}


			return userInfo;
		}

		public  Task<UserInfo> GetById(Guid id)
		{
			return Task.FromResult(UserDb.GetById(id));
		}

		public Task<IEnumerable<UserInfo>> Search(List<Expression<Func<UserInfo, bool>>> whereExpressions, int pageSize, int pageIndex)
		{
			throw new NotImplementedException();
		}

		public async Task<UserRole> BindRole(UserRole userRole)
		{
			var validateResults = UserRole.Validate(userRole);
			var current = UserRoleDb.GetSingle(ur => ur.UserId == userRole.UserId & ur.RoleId == userRole.RoleId);
			if (validateResults.All(u => u.IsSuccess) && current.IsNull())
			{
				userRole.Id = Guid.NewGuid();
				await UserRoleDb.AsInsertable(userRole).ExecuteCommandAsync();
			}

			return userRole;
		}

		public Task<bool> UnbindRole(Guid userRoleId)
		{
			var result = false;
			var userRole =  UserRoleDb.GetById(userRoleId);
			if (!userRole.IsNull())
			{
				result=UserRoleDb.DeleteById(userRoleId);
			}

			return Task.FromResult(result);
		}

		public async Task<IEnumerable<UserRole>> SearchUserRole(Guid userId, Guid roleId)
		{
			return await UserRoleDb.AsQueryable().Where(ur=>ur.UserId==userId & ur.RoleId==roleId).WithCache().ToListAsync();
		}

		private readonly IRedisCache _cache;

	}

}
