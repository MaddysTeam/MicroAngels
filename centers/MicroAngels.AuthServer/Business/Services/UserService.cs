using Business.Helpers;
using DotNetCore.CAP;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using MicroAngels.ServiceDiscovery.Consul;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

		public async Task<IEnumerable<UserInfo>> SearchWithFriends(UserSearchOption option, PageOptions page)
		{
			var users = await Search(null, page);
			var friends = new List<FriendViewModel>();
			using (var client = new HttpClient())
			{
				var fromService = _conf["FriendService:From"];
				var virtualPath = _conf["FriendService:VirtualPath"];
				friends = await client.PostAsync<List<FriendViewModel>, ConsulService>(fromService, virtualPath, null, new { SubscriberId = option.UserId, ServiceId = option.ServiceId }, _serviceFinder, _loadBalancer);
				foreach (var friend in friends)
				{
					var user = users.FirstOrDefault(x => x.UserId == friend.TargetId.ToGuid());
					if (!user.IsNull())
						user.IsFriend = true;
				}
			}

			return users;
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

		public async Task SendAddAccountMessage(UserInfo info)
		{
			await _publisher.PublishAsync(new AddAccountMessage
			{
				 Body=info.ToJson(),
				 Email=info.Email,
				 Name=info.UserName,
				 HasTrans=false,
				 Phone=info.Phone
			});
		}

		[CapSubscribe(AppKeys.AddUser,Group = AppKeys.AddUser)]
		public async Task ReceiveAddUserMessage(string message)
		{
			AddUserMessage msg = JsonConvert.DeserializeObject<AddUserMessage>(message);
			if (!msg.IsNull())
			{
				await Edit(new UserInfo { UserName=msg.UserName, Email=msg.Email, Phone=msg.Phone  } );
			}
		}

		private ICAPPublisher _publisher;
		private IServiceFinder<ConsulService> _serviceFinder;
		private ILoadBalancer _loadBalancer;
		private IConfiguration _conf;

	}

}
