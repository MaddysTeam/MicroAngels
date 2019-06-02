using FileService.Business;
using MicroAngels.GRPC.Client;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResourceService.Business
{

	public class CroResourceService : MySqlDbContext, ICroResourceService
	{

		public CroResourceService(IGRPCConnection connection, GRPCService grpcService)
		{
			_connection = connection;
			_grpcService = grpcService;
		}

		public async Task<bool> Edit(CroResource resource)
		{
			var results = CroResource.Validate(resource);
			if (results.TrueForAll(x => x.IsSuccess))
			{
				//TODO: will get file data from file service by using fileId
				var fileService = _connection.GetGRPCService<IFileGRPCService>(string.Empty).Result;
				var fileExist= await fileService.FileExist(resource.FildId).ResponseAsync;
				if (fileExist)
				{
					ResourceDb.Insert(resource);
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		public CroResource Get(Guid id)
		{
			return ResourceDb.GetById(id);
		}

		public async Task<List<CroResource>> Search(List<Expression<Func<CroResource, bool>>> whereExpressions, int pageSize, int pageIndex)
		{
			var query = ResourceDb.AsQueryable();

			foreach(var exp in whereExpressions)
			{
				query.Where(exp);
			}

			var pageCount = query.Count();

			return pageSize <= 0 ? await query.ToListAsync() : await query.ToPageListAsync(pageIndex, pageSize);
		}


		private readonly IGRPCConnection _connection;
		private readonly GRPCService _grpcService;

	}

}
