using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using Microsoft.AspNetCore.Http;

namespace FileService.Business
{

	public class FileService : MySqlDbContext, IFileService
	{

		public FileService(ICAPPublisher bus)
		{
			_bus = bus;
		} 

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

		public Task<List<Files>> UploadFilesAsync(IFormFileCollection formFiles, string savePath)
		{
			throw new NotImplementedException();
		}


		private readonly ICAPPublisher _bus;

	}

}
