using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Hystrix;
using MicroAngels.Hystrix.Polly;
using Microsoft.AspNetCore.Http;

namespace FileService.Business
{

	public class FileService : MySqlDbContext, IFileService
	{

		public FileService()
		{
			//_bus = bus;
		}

		/// <summary>
		/// uplolad files
		/// </summary>
		/// <param name="formFiles"></param>
		/// <param name="foler"></param>
		/// <returns></returns>
		public List<Files> UploadFiles(IFormFileCollection formFiles, string foler)
		{
			var uploadFiles = new List<Files>();
			foreach (var file in formFiles)
			{
				if (file.Length > 0)
				{
					var ext = Path.GetExtension(file.FileName);
					var filePath = Path.Combine(foler, $@"{ foler}/{Guid.NewGuid()}{ext}");
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						file.CopyTo(stream);

						var md5 = MD5Cryptor.Encrypt(stream);
						var existFile = db.GetSingle(f => f.MD5 == md5);
						if (!existFile.IsNull())
						{
							var fileObj = new Files
							{
								FildId = Guid.NewGuid(),
								FileName = file.Name,
								FilePath = filePath,
								FileExtension = ext,
								FileSize = file.Length
							};

							db.Insert(fileObj);
							uploadFiles.Add(fileObj);
						}
						else
						{
							uploadFiles.Add(existFile);
						}

						//_bus.PublishAsync(new CAPMessage())
					}
				}
			}

			return uploadFiles;
		}

		/// <summary>
		/// upload files by asynchronous method
		/// </summary>
		/// <param name="formFiles"></param>
		/// <param name="savePath"></param>
		/// <returns></returns>
		public Task<List<Files>> UploadFilesAsync(IFormFileCollection formFiles, string savePath)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// search files
		/// </summary>
		/// <param name="whereExpressions"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		//[Polly(nameof(GetFilesFallback), IsEnableCircuitBreaker = true, ExceptionsAllowedBeforeBreaking = 2)]
		public List<Files> Search(List<Expression<Func<Files, bool>>> whereExpressions, int? pageSize, int? pageIndex , out int totalCount)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// call back service. TODO: comment for temp
		/// </summary>
		/// <returns></returns>
		//public Task<List<Files>> GetFilesFallback(List<Expression<Func<Files, bool>>> whereExpressions, int? pageSize, int? pageIndex)
		//{
		//	return Task.FromResult(new List<Files>());
		//}

		private readonly ICAPPublisher _bus;

	}

}
