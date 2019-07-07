using Business;
using MicroAngels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
	public class SystemService : MySqlDbContext, ISystemService
	{

		public SystemService()
		{

		}

		public async Task<bool> Edit(System system)
		{
			int result = 0;
			var validateResults = System.Validate(system);
			if (validateResults.All(u => u.IsSuccess))
			{
				
				if (system.SystemId.IsEmpty())
				{
					var isExists = !SystemDb.GetSingle(sys => sys.SystemName.IsSame(system.SystemName)).IsNull();
					if (isExists) return false;

					result = await SystemDb.AsInsertable(system).ExecuteCommandAsync();
				}
				else
				{
					result = await SystemDb.AsUpdateable(system).ExecuteCommandAsync();
				}
			}

			return result > 0;
		}

		public Task<System> GetById(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
