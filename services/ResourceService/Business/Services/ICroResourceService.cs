using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceService.Business
{

	public interface ICroResourceService
	{
		List<CroResource> SearchResourceList(List<System.Linq.Expressions.Expression<Func<CroResource, bool>>> whereExpressions, int pageSize, int pageIndex);
		bool EditResource(CroResource resource);
		CroResource GetResource(Guid id);
	}

}
