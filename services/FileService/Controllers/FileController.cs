using FileService.Business;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace FileService.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{

		public FileController(IHostingEnvironment hosting, IFileService fileService)
		{
			_folder = $@"{hosting.ContentRootPath}\uploader";
			_fileService = fileService;
		}

		[Route("upload")]
		[HttpPost()]
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

		private string _folder;
		private IFileService _fileService;

	}

}
