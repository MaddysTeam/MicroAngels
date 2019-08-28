using MicroAngels.Cache;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IUserService : IUserCaching
	{
		//[Caching(AbsoluteExpiration = 10, ActionType = ActionType.search, IsAsync = true)]
		Task<UserInfo> GetById(Guid id);
		//[Caching(AbsoluteExpiration = 10, ActionType = ActionType.search, IsAsync = true)]
		Task<UserInfo> GetByName(string name);
		Task<bool> Edit(UserInfo userInfo);
		Task<bool> BindRoles(Guid userId, string[] roleIds);
		Task<IEnumerable<UserInfo>> Search(Expression<Func<UserInfo, bool>> whereExpressions, PageOptions page);

		[Caching(AbsoluteExpiration = 10, ActionType = ActionType.search, IsAsync = true, TargetType =typeof(IEnumerable<UserInfo>))]
		Task<IEnumerable<UserInfo>> SearchWithFriends(UserSearchOption option, PageOptions page);
	}

}
