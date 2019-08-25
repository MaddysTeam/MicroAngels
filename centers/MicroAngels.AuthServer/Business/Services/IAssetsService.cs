using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IAssetsService
	{
		Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames);
		IEnumerable<Interface> SearchInterface(Expression<Func<Interface, bool>> whereExpressions, PageOptions page);
		IEnumerable<Menu> SearchMenu(Expression<Func<Menu, bool>> whereExpressions, PageOptions page);
		Task<IEnumerable<Menu>> GetMenusByRoleNames(string[] roleNames);
		Task<IEnumerable<Menu>> GetMenusByUserId(Guid userId);
		Task<IEnumerable<Assets>> SearchAssets(Expression<Func<Assets, bool>> whereExpressions);
		Task<IEnumerable<Assets>> GetRoleAssets(Guid roleId);
		Task<bool> Edit(Assets assets);
		Task<bool> MultiEdit(List<Assets> assetsList);
		Task<bool> EditInterface(Interface iinterface);
		Task<bool> EditMenu(Menu menu);
		Task<bool> BindAssets(Guid assetId, Guid itemId);
		Task<Assets> GetById(Guid assetId);
		Task<Menu> GetMenuById(Guid menuId);
		Task<Interface> GetInterfaceById(Guid interfaceId);
	}

}
