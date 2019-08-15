using Business;
using Business.Services;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class TopicController : BaseController
	{

		public TopicController(
			ILogger logger,
			ITopicService topicService
			):base()
		{
			_logger = logger;
			_topicService = topicService;
		}

		[HttpPost("edit")]
		public async Task<IActionResult> EditTopic([FromForm] TopicViewModel topicViewModel)
		{
			var topic = Mapper.Map<TopicViewModel, Topic>(topicViewModel); 
			if (topic.IsNull() || !topic.IsValidate || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var isSuccess = await _topicService.EditTopicAsync(topic);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("topics")]
		public IActionResult GetTopics([FromForm]int? start, [FromForm] int? length)
		{
			var totalCount = 0;
			var topics =  _topicService.Search(null, start, length, out totalCount);
			
			if (!topics.IsNull() && topics.Count() > 0)
			{
				return new JsonResult(new
				{
					data = topics.Select(t => Mapper.Map<Topic, TopicViewModel>(t)),
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

		[HttpPost("info")]
		public async Task<IActionResult> GetTopic([FromForm] string id)
		{
			var topicObj = await _topicService.GetTopicAsync(id);

			return new JsonResult(new
			{
				data = Mapper.Map<Topic, TopicViewModel>(topicObj)
			});
		}


		private readonly ILogger _logger;
		private readonly ITopicService _topicService;

	}
}
