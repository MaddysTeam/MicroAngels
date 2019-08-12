using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Services
{

	public interface ISubscribeService
    {
        Task<bool> SubscribeAsync(Subscribe subscribe);
        Task<bool> UnSubsribeAsync(Subscribe subscribe);     
        Task<List<Subscribe>> GetSubscribes(Expression<Func<Subscribe, bool>> whereExpressions, int? pageIndex, int? pageSize);
    }

   
}
