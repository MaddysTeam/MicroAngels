using Microsoft.AspNetCore.Mvc;

namespace MicroAngels.Core.Plugins
{

	public class AngleBaseController: ControllerBase
	{
		public UserContext CurrnetUser { get; }

		public AngleBaseController()
		{
			CurrnetUser = new UserContext(HttpContext);
		}

	}

}
