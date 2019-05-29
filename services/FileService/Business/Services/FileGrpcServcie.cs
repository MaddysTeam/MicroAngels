using MagicOnion;
using MagicOnion.Server;
using MicroAngels.Core;
using ResourceService.Business;
using System;

namespace FileService.Business.Services
{
	public class FileGrpcServcie : ServiceBase<IFileGRPCService>, IFileGRPCService
	{
		
		public FileGrpcServcie(MySqlDbContext context)
		{
			//context.EnsureNotNull(() => new Aruge { });

			_dBcontext = context;
		}

		public async UnaryResult<Files> GetFileById(Guid fileId)
		{
			return _dBcontext.db.GetById(fileId);
		}


		private readonly MySqlDbContext _dBcontext;

	}
}
