using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserService.Business
{

	public interface IRoleService
	{
		Task<SystemRole> GetById(Guid id);
		Task<SystemRole> Edit(SystemRole role);
		Task<RolePrivilege> BindPrivilege(RolePrivilege rolePrivilege);
		Task<bool> UnbindPrivilege(Guid id);
		Task<IEnumerable<SystemRole>> Search(List<Expression<Func<SystemRole, bool>>> whereExpressions, int pageSize, int pageIndex);
	}

}
