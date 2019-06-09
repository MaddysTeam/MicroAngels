using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Business
{

	public interface IPrivilegeService
	{
		Task<SystemRole> GetById(Guid id);
		Task<SystemRole> Edit(SystemRole role);
	}

}
