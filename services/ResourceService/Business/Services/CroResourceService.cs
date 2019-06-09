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

		/// <summary>
		/// Edit Resource
		/// </summary>
		/// <param name="resource">resource instance</param>
		/// <returns>async bool</returns>
		public async Task<bool> Edit(Resource resource)
		{
			var results = Resource.Validate(resource);
			if (results.TrueForAll(x => x.IsSuccess))
			{
				//TODO: will get file data from file service by using fileId
				var fileService = _connection.GetGRPCService<IFileGRPCService>(_grpcService).Result;
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

		public Resource Get(Guid id)
		{
			return ResourceDb.GetById(id);
		}

		public async Task<List<Resource>> Search(List<Expression<Func<Resource, bool>>> whereExpressions, int pageSize, int pageIndex)
		{
			var query = ResourceDb.AsQueryable();

			foreach(var exp in whereExpressions)
			{
				query.Where(exp);
			}

			var pageCount = query.Count();

			return pageSize <= 0 ? await query.ToListAsync() : await query.ToPageListAsync(pageIndex, pageSize);
		}

		public async Task<bool> Favorite(Guid id, Guid userId)
		{
			//FavoriteDB.Insert(new ResourceFavorite(Guid.NewGuid(), id, userId));

			return false;
		}

		private readonly IGRPCConnection _connection;
		private readonly GRPCService _grpcService;

	}

}
