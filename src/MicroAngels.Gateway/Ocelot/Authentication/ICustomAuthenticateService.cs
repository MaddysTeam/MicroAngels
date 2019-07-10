using Ocelot.Middleware;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

	public interface ICustomAuthenticateService
	{
		Task<bool> ValidateAuthenticate(DownstreamContext context);
	}

}
