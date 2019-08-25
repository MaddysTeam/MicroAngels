﻿using MicroAngels.Hystrix;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileService.Business
{

	public interface IFileService
	{
		List<Files> UploadFiles(IFormFileCollection formFiles);

		Task<List<Files>> UploadFilesAsync(IFormFileCollection formFiles);

		List<Files> Search(List<System.Linq.Expressions.Expression<Func<Files, bool>>> whereExpressions, int? pageSize, int? pageIndex, out int totalCount);
	}

}
