using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public class UserService : MySqlDbContext, IUserService
	{

		public UserService(ICAPPublisher publisher)
		{
			_publisher = publisher;
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
			return await UserDb.AsQueryable().FirstAsync(u=>u.UserId==id);
		}

		public async Task<IEnumerable<UserInfo>> Search(Expression<Func<UserInfo, bool>> whereExpressions, PageOptions page)
		{
			
			var query = whereExpressions.IsNull() ? UserDb.AsQueryable() : UserDb.AsQueryable().Where(whereExpressions);

			if (!page.IsNull() && page.IsValidate)
			{
				var results= await query.ToPageListAsync(page.PageIndex, page.PageSize);

				page.TotalCount = query.Count();

				return results;
			}
			else
				return query.ToList();
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
				result = await UserRoleDb.AsDeleteable().ExecuteCommandAsync()>0;
			}

			return result;
		}

		public async Task<UserInfo> GetByName(string name)
		{
			var user =await UserDb.AsQueryable().FirstAsync(u => string.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase));
			return user;
		}

		public async Task<bool> Focus(Guid userId,Guid targetId)
		{
			var mo = new  { Topic = "", ServiceId = Keys.System.DefaultSystemId, SenderId = userId, SendTime = DateTime.Now, SubscriberId = userId, TargetId = targetId };
			var message = Newtonsoft.Json.JsonConvert.SerializeObject(mo);

			await _publisher.PublishAsync(new CAPMessage("MessageCenter.Subscribe", message, false));

			return true;
		}

		private ICAPPublisher _publisher;

	}

}
