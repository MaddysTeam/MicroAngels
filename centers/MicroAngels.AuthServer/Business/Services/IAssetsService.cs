using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IAssetsService
	{
		Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames);
		Task<bool> EditAsset(Assets assets);
		Task<bool> EditInterface(Interface iinterface);
	}

}
