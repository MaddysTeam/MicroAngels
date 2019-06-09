using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserService.Business
{

	public interface IUserService
	{
		Task<UserInfo> GetById(Guid id);
		Task<UserInfo> Edit(UserInfo userInfo);
		Task<UserRole> BindRole(UserRole userRole);
		Task<bool> UnbindRole(Guid userRoleId);
		Task<IEnumerable<UserInfo>> Search(List<Expression<Func<UserInfo, bool>>> whereExpressions, int pageSize, int pageIndex);
		Task<IEnumerable<UserRole>> SearchUserRole(Guid userId,Guid roleId);
	}

}
