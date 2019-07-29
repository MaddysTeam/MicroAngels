using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IAssetsService
	{
		Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames);
		IEnumerable<Interface> SearchInterface(Expression<Func<Interface, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount);
		IEnumerable<Menu> SearchMenu(Expression<Func<Menu, bool>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount);
		Task<IEnumerable<Menu>> GetMenusByRoleNames(string[] roleNames);
		Task<IEnumerable<Menu>> GetMenusByUserId(Guid userId);
		Task<IEnumerable<Assets>> SearchAssets(Expression<Func<Assets, bool>> whereExpressions);
		Task<IEnumerable<Assets>> GetRoleAssets(Guid roleId);
		Task<bool> Edit(Assets assets);
		Task<bool> EditInterface(Interface iinterface);
		Task<bool> EditMenu(Menu menu);
		Task<bool> BindAssets(Guid assetId, Guid itemId);
		Task<Assets> GetById(Guid assetId);
		Task<Menu> GetMenuById(Guid menuId);
	}

}
