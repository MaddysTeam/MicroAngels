using MagicOnion;
using MagicOnion.Server;
using MicroAngels.Core;
using System;

namespace FileService.Business.Services
{
	public class FileGrpcServcie : ServiceBase<IFileGRPCService>, IFileGRPCService
	{

		public FileGrpcServcie() { }

		public FileGrpcServcie(MySqlDbContext context)
		{
			context.EnsureNotNull(() => new AngleExceptions { });

			_dBcontext = context;
		}

		public async UnaryResult<Files> GetFileById(Guid fileId)
		{
			return await _dBcontext
						.db
						.AsQueryable()
						.SingleAsync(x => x.FildId == fileId);
		}

		public async UnaryResult<bool> FileExist(Guid fileId)
		{
			return await _dBcontext
						.db
						.AsQueryable()
						.CountAsync(f => f.FildId == fileId) > 0;

		}

		private readonly MySqlDbContext _dBcontext;

	}
}
