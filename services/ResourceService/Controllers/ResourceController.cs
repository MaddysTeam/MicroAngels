﻿using MicroAngels.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Business;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResourceService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResourceController : ControllerBase
	{

		public ResourceController(ICroResourceService resourceService)
		{
			_resourceService = resourceService;
		}

		[Authorize]
		[HttpPost]
		[Route("list")]
		public async Task<List<Resource>> List(Guid userId,int pageSize,int pageIndex)
		{
			var whereExpressions = new List<Expression<Func<Resource, bool>>>();

			if (!userId.IsEmpty())
			{
				whereExpressions.Add(x=>x.Creator== userId);
			}

			var result = await _resourceService.Search(whereExpressions, pageSize, pageIndex);

			return result ;
		}


		[Authorize]
		[HttpPost]
		[Route("Edit")]
		public IActionResult Edit(Resource croResource)
		{
			croResource.EnsureNotNull(() => new AngleExceptions(""));

			var result=_resourceService.Edit(croResource);

			return Ok(new { isSuccess= result, message="" });
		}


		[Authorize]
		[HttpPost]
		[Route("Favorite")]
		public IActionResult Favorite(Guid id,Guid userId)
		{
			id.EnsureNotEmpty(() => new AngleExceptions(""));
			userId.EnsureNotEmpty(() => new AngleExceptions(""));

			_resourceService.Favorite(id, userId);

			return null;
		}


		private readonly ICroResourceService _resourceService;

	}
}
