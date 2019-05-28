using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Business;
using System.Collections.Generic;

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
		public List<CroResource> List()
		{
			return null;
		}


		[Authorize]
		[HttpPost]
		[Route("Edit")]
		public List<CroResource> Edit(CroResource croResource)
		{
			return null;
		}

		private readonly ICroResourceService _resourceService;


		// GET api/values
		//[HttpGet]
		//public ActionResult<IEnumerable<string>> Get()
		//{
		//	return new string[] { "value1", "value2" };
		//}

		//// GET api/values/5
		//[HttpGet("{id}")]
		//public ActionResult<string> Get(int id)
		//{
		//	return "value";
		//}

		//// POST api/values
		//[HttpPost]
		//public void Post([FromBody] string value)
		//{
		//}

		//// PUT api/values/5
		//[HttpPut("{id}")]
		//public void Put(int id, [FromBody] string value)
		//{
		//}

		//// DELETE api/values/5
		//[HttpDelete("{id}")]
		//public void Delete(int id)
		//{
		//}
	}
}
