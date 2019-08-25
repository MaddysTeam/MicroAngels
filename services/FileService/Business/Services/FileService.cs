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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileService.Business
{

	public class FileService : MySqlDbContext, IFileService
	{

		public FileService(IHostingEnvironment hostingEnvironment)
		{
			//_bus = bus
			_env = hostingEnvironment;
		}

		/// <summary>
		/// uplolad files
		/// </summary>
		/// <param name="formFiles"></param>
		/// <param name="foler"></param>
		/// <returns></returns>
		public List<Files> UploadFiles(IFormFileCollection formFiles)
		{
			var uploadFiles = new List<Files>();
			foreach (var file in formFiles)
			{
				if (file.Length > 0)
				{
					var filePath = GetFilePath();
					if (!Directory.Exists(filePath))
					{
						Directory.CreateDirectory(filePath);
					}

					var fileName = GetFileName(file.FileName);
					var fileWebPath = filePath + fileName;
					using (var stream = new FileStream(fileWebPath, FileMode.Create))
					{
						file.CopyTo(stream);

						var md5 = MD5Cryptor.Encrypt(stream);

						//var existFile = db.GetSingle(f => f.MD5 == md5);
						//if (existFile.IsNull())
						//{
						var fileObj = new Files
						{
							FildId = Guid.NewGuid(),
							FileName = file.Name,
							FilePath = StaticFilePath().Replace("\\","/")+ fileName,
							FileExtension = Path.GetExtension(file.FileName),
							FileSize = file.Length,
							MD5 = md5
						};

						db.Insert(fileObj);
						uploadFiles.Add(fileObj);
						//}
						//else
						//{
						//	uploadFiles.Add(existFile);
						//}

						//_bus.PublishAsync(new CAPMessage())
					}
				}
			}

			return uploadFiles;
		}


		private string StaticFilePath()=> @"\" + "StaticFiles" + @"\" + DateTime.Now.ToString("yyMMdd") + @"\";

		private string GetFilePath()
		{
			return _env.ContentRootPath + StaticFilePath();
		}

		private string GetFileName(string fileName)
		{
			return DateTime.Now.ToString("yyMMddHHmmss") + Path.GetExtension(fileName);
		}

		/// <summary>
		/// upload files by asynchronous method
		/// </summary>
		/// <param name="formFiles"></param>
		/// <param name="savePath"></param>
		/// <returns></returns>
		public Task<List<Files>> UploadFilesAsync(IFormFileCollection formFiles)
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
		public List<Files> Search(List<Expression<Func<Files, bool>>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount)
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
		private readonly IHostingEnvironment _env;

	}

}
