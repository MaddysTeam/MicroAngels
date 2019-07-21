﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IUserService
	{
		Task<UserInfo> GetById(Guid id);
		UserInfo GetByName(string name);
		Task<bool> Edit(UserInfo userInfo);
		Task<bool> BindRole(UserRole userRole);
		Task<bool> UnbindRole(Guid userRoleId);
		IEnumerable<UserInfo> Search(Expression<Func<UserInfo, bool>> whereExpressions, int? pageSize, int? pageIndex,out int totalCount);
	}

}
