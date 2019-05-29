using MagicOnion;
using System;

namespace FileService.Business
{

	public interface IFileGRPCService:IService<IFileGRPCService>
	{
		UnaryResult<Files> GetFileById(Guid fileId);
	}

}
