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
		Task<bool> UnbindResource(Guid roleId,Guid assetsId);
		Task<IEnumerable<SystemRole>> Search(Expression<Func<SystemRole, bool>> whereExpressions, int? pageSize, int? pageIndex);
		Task<List<SystemRole>> GetByUserName(string userName);
	}

}
