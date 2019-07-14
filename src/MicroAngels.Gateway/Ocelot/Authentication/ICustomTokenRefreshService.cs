using Ocelot.Middleware;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public interface ICustomTokenRefreshService
	{
		Task<DownstreamContext> Refresh(DownstreamContext context);
	}

}
