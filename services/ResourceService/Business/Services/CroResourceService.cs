using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ResourceService.Business
{

	public class CroResourceService : ICroResourceService
	{

		public bool EditResource(CroResource resource)
		{
			throw new NotImplementedException();
		}

		public CroResource GetResource(Guid id)
		{
			throw new NotImplementedException();
		}

		public List<CroResource> SearchResourceList(List<Expression<Func<CroResource, bool>>> whereExpressions, int pageSize, int pageIndex)
		{
			throw new NotImplementedException();
		}

	}

}
