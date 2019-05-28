using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ResourceService.Business
{

	public class CroResourceService : MySqlDbContext, ICroResourceService
	{

		public bool EditResource(CroResource resource)
		{
			var results = CroResource.Validate(resource);
			if (results.TrueForAll(x => x.IsSuccess))
			{
				//TODO: will get file data from file service by using fileId

				ResourceDb.Insert(resource);

				return true;
			}

			return false;
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
