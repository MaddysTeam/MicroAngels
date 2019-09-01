using Microsoft.AspNetCore.Http;

namespace MicroAngels.Core.Plugins
{

	public class UserContext
	{
		public string UserId => Current.User.GetClaimsValue(CoreKeys.USER_ID);
		public string ServiceId => Current.User.GetClaimsValue(CoreKeys.SYSTEM_ID);

		public UserContext(HttpContext context)
		{
			if (context.IsNull())
				throw new AngleExceptions("http context instance cannot be null");

			Current = context;
		}

		protected HttpContext Current { get; set; }

	}

}
