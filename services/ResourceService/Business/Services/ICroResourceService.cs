using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceService.Business
{

	public interface ICroResourceService
	{
		Task<List<Resource>> Search(List<System.Linq.Expressions.Expression<Func<Resource, bool>>> whereExpressions, int pageSize, int pageIndex);
		Task<bool> Edit(Resource resource);
		Task<bool> Favorite(Guid id, Guid userId);
		Resource Get(Guid id);
	}

}
