using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceService.Business
{

	public interface ICroResourceService
	{
		Task<List<CroResource>> Search(List<System.Linq.Expressions.Expression<Func<CroResource, bool>>> whereExpressions, int pageSize, int pageIndex);
		Task<bool> Edit(CroResource resource);
		CroResource Get(Guid id);
	}

}
