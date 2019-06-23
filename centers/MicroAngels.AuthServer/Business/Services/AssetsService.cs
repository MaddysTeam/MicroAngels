using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business
{

	public class AssetsService : MySqlDbContext, IAssetsService
	{

		public AssetsService()
		{
		}

		public Task<IEnumerable<Interface>> GetInterfaceByRoleNames(string[] roleNames)
		{
			throw new NotImplementedException();
		}

	}

}
