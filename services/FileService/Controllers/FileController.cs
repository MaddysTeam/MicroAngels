using FileService.Business;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FileService.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{

		public FileController(IFileService fileService)
		{
			_fileService = fileService;
		}

		[HttpPost("upload")]
		public ActionResult Upload()
		{
			var uploadFiles = _fileService.UploadFiles(Request.Form.Files);

			if (uploadFiles.Count == 1)
			{
				return new JsonResult(new
				{
					path = uploadFiles.FirstOrDefault().FilePath
				});
			}

			return new JsonResult(new
			{
			});

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
