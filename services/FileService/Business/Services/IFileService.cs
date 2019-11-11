using Business;
using MicroAngels.Hystrix;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileService.Business
{

	public interface IFileService
	{
		List<Files> UploadFiles(IFormFileCollection formFiles);

		Task<List<Files>> Search(List<Expression<Func<Files, bool>>> whereExpressions, PageOptions page);

		Task<List<Files>> SearchFallback(List<Expression<Func<Files, bool>>> whereExpressions, PageOptions page);
	}

}
