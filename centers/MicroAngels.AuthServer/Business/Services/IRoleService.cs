using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IRoleService
	{
		Task<SystemRole> GetById(Guid id);
		Task<bool> Edit(SystemRole role);
		Task<bool> BindResource(RoleAssets roleAccess);
		Task<bool> UnbindResource(Guid roleId, Guid assetsId);
		IEnumerable<SystemRole> Search(Expression<Func<SystemRole, bool>> whereExpressions, PageOptions page);
		Task<List<SystemRole>> GetByUserName(string userName);
		Task<List<SystemRole>> GetByUserIds(Guid[] userIds);
		Task<List<SystemRole>> GetByUserId(Guid userid);
	}

}
