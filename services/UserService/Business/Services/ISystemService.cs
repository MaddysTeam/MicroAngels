using MicroAngels.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Business
{

	public interface ISystemService 
	{
		Task<System> GetById(Guid id);
		Task<System> Edit(System system);
	}

}
