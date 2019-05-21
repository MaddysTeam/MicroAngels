using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace MicroAngels.Logger
{

	/// <summary>
	/// this log executor in filter context, you can implement it for own business 
	/// </summary>
	public interface IFilterLogExecutor
	{
		void Execute<Context>(Context ctx) where Context:FilterContext;
		Task ExecuteAsync<Context>(Context ctx) where Context : FilterContext;
	}

}
