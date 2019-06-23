using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AssetsController : ControllerBase
	{

		public AssetsController()
		{

		}

		[HttpGet]
		public async Task<string[]> GetUrls(string[] role)
		{
			return null;
		}

		private readonly IAssetsService _service;
		
	}

}
