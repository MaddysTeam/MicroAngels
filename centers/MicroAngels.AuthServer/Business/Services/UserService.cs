using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business
{

	public class UserService : MySqlDbContext, IUserService
	{

		public UserService(ICAPPublisher publisher, IServiceFinder<ConsulService> serviceFinder, ILoadBalancer loadBalancer, IConfiguration configuration)
		{
			_publisher = publisher;
			_loadBalancer = loadBalancer;
			_serviceFinder = serviceFinder;
			_conf = configuration;
		}

		public async Task<bool> Edit(UserInfo userInfo)
		{
			if (UserInfo.Validate(userInfo).All(u => u.IsSuccess))
			{
				if (userInfo.UserId.IsEmpty())
				{
					var current = UserDb.GetSingle(ur => ur.UserName == userInfo.UserName);
					if (current.IsNull())
					{
						userInfo.UserId = Guid.NewGuid();
						return await UserDb.AsInsertable(userInfo).ExecuteCommandAsync() > 0;
					}
					else
						return false;
				}
				else
					return await UserDb.AsUpdateable(userInfo).ExecuteCommandAsync() > 0;
			}

			return false;
		}

		public async Task<UserInfo> GetById(Guid id)
		{
			return await UserDb.AsQueryable().FirstAsync(u => u.UserId == id);
		}

		public async Task<IEnumerable<UserInfo>> Search(Expression<Func<UserInfo, bool>> whereExpressions, PageOptions page)
		{

			var query = whereExpressions.IsNull() ? UserDb.AsQueryable() : UserDb.AsQueryable().Where(whereExpressions);
			var results = new List<UserInfo>();

			if (!page.IsNull() && page.IsValidate)
			{
				results = await query.ToPageListAsync(page.PageIndex, page.PageSize);

				page.TotalCount = query.Count();
			}
			else
				results = await query.ToListAsync();

			return results;
		}

		public async Task<bool> BindRoles(Guid userId, string[] roleIds)
		{
			bool result = false;

			await UserRoleDb.AsDeleteable().Where(ur => ur.UserId == userId).ExecuteCommandAsync();

			foreach (var roleId in roleIds)
			{
				var userRole = new UserRole() { Id = Guid.NewGuid(), RoleId = roleId.ToGuid(), UserId = userId };
				result = await UserRoleDb.AsInsertable(userRole).ExecuteCommandAsync() > 0;
			}

			return result;
		}

		public async Task<bool> UnbindRole(Guid userRoleId)
		{
			var result = false;
			var userRole = UserRoleDb.GetById(userRoleId);
			if (!userRole.IsNull())
			{
				result = await UserRoleDb.AsDeleteable().ExecuteCommandAsync() > 0;
			}

			return result;
		}

		public async Task<UserInfo> GetByName(string name)
		{
			var user = await UserDb.AsQueryable().FirstAsync(u => string.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase));
			return user;
		}

		public async Task<bool> Focus(Guid userId, Guid targetId)
		{
			var mo = new { Topic = "", ServiceId = Keys.System.DefaultSystemId, SenderId = userId, SendTime = DateTime.Now, SubscriberId = userId, TargetId = targetId };
			var message = Newtonsoft.Json.JsonConvert.SerializeObject(mo);

			await _publisher.PublishAsync(new CAPMessage("MessageCenter.Subscribe", message, false));

			return true;
		}

		public async Task<IEnumerable<UserInfo>> SearchFriends(Guid id, string code, PageOptions page)
		{
			var users = await Search(null, page);
			var friends = new List<FriendViewModel>();
			using (var client = new HttpClient())
			{
				var fromService = _conf["FriendService:From"];
				var virtualPath = _conf["VirtualPath"];
				friends = await client.PostAsync<List<FriendViewModel>, ConsulService>(fromService, virtualPath, code, null, _serviceFinder, _loadBalancer);
				foreach (var friend in friends)
				{
					var user = users.FirstOrDefault(x => x.UserId == friend.Id.ToGuid());
					user.IsFriend = !user.IsNull();
				}
			}

			return users;
		}

		private ICAPPublisher _publisher;
		private IServiceFinder<ConsulService> _serviceFinder;
		private ILoadBalancer _loadBalancer;
		private IConfiguration _conf;

	}

}
