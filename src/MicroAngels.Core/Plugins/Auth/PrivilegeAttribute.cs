using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.Core.Plugins.Auth
{

	public class PrivilegeAttribute : TypeFilterAttribute
	{

		public PrivilegeAttribute(string url = null) :
			base(typeof(PrivilegeExecutor))
		{
			Arguments = new object[] { new PrivilegeRequirement(url) };
		}

	}

	public class PrivilegeExecutor : ActionFilterAttribute
	{
		private PrivilegeRequirement _requirement;
		private IPrivilegeSupplier _permissions;

		public PrivilegeExecutor(PrivilegeRequirement requirement, IPrivilegeSupplier permissions)
		{
			_requirement = requirement;
			_permissions = permissions;
		}

		public override async void OnActionExecuting(ActionExecutingContext context)
		{
			var roleClaims = context.HttpContext.User.Claims.Where(c => c.Value == "role");
			if (roleClaims.Count() > 0)
			{
				var requireUrl = _requirement.Url;
				if (string.IsNullOrEmpty(requireUrl))
				{
					requireUrl = context.HttpContext.Request.Path;
				}

				var roles = roleClaims.Select(x => x.Type).ToArray();
				var permissions = await _permissions.GetUrls(roles);
				var hasPermission = permissions?.FirstOrDefault(url => url == requireUrl).IsNullOrEmpty();
				if (hasPermission.IsNull() || hasPermission.Value)
				{
					context.Result = new ContentResult() { StatusCode = StatusCodes.Status401Unauthorized, Content = "permission deny" };
				}
			}
			else
				context.Result = new ContentResult() { StatusCode = StatusCodes.Status401Unauthorized, Content = "need auth first" };
		}
	}

	public class PrivilegeRequirement
	{
		public string Url { get; }

		public PrivilegeRequirement(string url)
		{
			Url = url;
		}
	}

	public interface IPrivilegeSupplier
	{
		Task<string[]> GetUrls(string[] role);
	}

}
