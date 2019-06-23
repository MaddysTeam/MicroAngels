using FileService.Business;
using MicroAngels.Core.Plugins.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileService.Controllers
{

	[Route("api/[controller]")]
	[Authorize]
	[Privilege]
	[ApiController]
	public class FileController : ControllerBase
	{

		public FileController(IHostingEnvironment hosting, IFileService fileService)
		{
			_folder = $@"{hosting.ContentRootPath}\uploader";
			_fileService = fileService;
		}

		
		[HttpPost("upload")]
		public ActionResult Upload()
		{
			if (!Directory.Exists(_folder))
				Directory.CreateDirectory(_folder);

			var uploadFiles = _fileService.UploadFiles(Request.Form.Files, _folder);

			return Ok(uploadFiles);
		}

		[HttpPost]
		[Route("getfile")]
		public ActionResult GetFile(Guid id)
		{
			return Ok();
		}


		[HttpPost]
		[Route("getfiles")]
		public List<Files> GetFiles()
		{
			return new List<Files>();
		}

		private string _folder;
		private IFileService _fileService;

	}

}
