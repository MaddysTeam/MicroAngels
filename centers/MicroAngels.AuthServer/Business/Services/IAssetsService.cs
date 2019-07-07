using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public interface IAssetsService
	{
		Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames);
		Task<bool> Edit(Assets assets);
		Task<bool> EditInterface(Interface iinterface);
		Task<bool> BindInterface(Guid assetId, Guid interfaceId);
		Task<Assets> GetById(Guid assetId);
	}

}
