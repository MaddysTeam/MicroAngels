using FileService.Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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

		[HttpPost("upload")]
		public ActionResult Upload()
		{
			if (!Directory.Exists(_folder))
				Directory.CreateDirectory(_folder);

			var uploadFiles = _fileService.UploadFiles(Request.Form.Files, _folder);

			return Ok(uploadFiles);
		}

		[HttpPost("file")]
		public ActionResult GetFile(Guid id)
		{
			return Ok();
		}

		[HttpPost("files")]
		public IActionResult GetFiles([FromForm]int start, [FromForm]int length)
		{
			//var results = await _fileService.GetFiles(null, null, null);
			var totalCount = 0;
			var searchResults = _fileService.Search(null, length, start, out totalCount);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(x => new
					{
					}),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,
				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}

		private string _folder;
		private IFileService _fileService;

	}

}
