using MicroAngels.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

	public interface ISystemService 
	{
		Task<System> GetById(Guid id);
		Task<Boolean> Edit(System system);
		
	}

}
