using Business;
using Business.Services;
using MicroAngels.Core;
using MicroAngels.Core.Plugins;
using MicroAngels.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class TopicController : ControllerBase
	{

		public TopicController(
			ILogger logger,
			ITopicService topicService
			)
		{
			_logger = logger;
			_topicService = topicService;
		}

		[HttpPost("edit")]
		public async Task<IActionResult> EditTopic([FromForm] Topic topic)
		{
			if (topic.IsNull() || !topic.IsValidate || !ModelState.IsValid)
			{
				// _logger.LogError(AlterKeys.Error.TOPIC_INVALID);

				return BadRequest(ModelState);
			}

			var isSuccess = await _topicService.EditTopicAsync(topic);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("list")]
		public async Task<IActionResult> GetTopics([FromForm]int pageSize, [FromForm] int pageIndex)
		{
			var totalCount = 0;
			var topics =  _topicService.Search(null, pageSize, pageIndex, out totalCount);
			var mapper = Mapper.Create(typeof(MapperProfile));
			if (topics.IsNull() && topics.Count() > 0)
			{
				return new JsonResult(new
				{
					data = topics.Select(t => mapper.Map<Topic, TopicViewModel>(t)),
					recordsTotal = totalCount,
					recordsFiltered = totalCount,
				});
			}

			return new JsonResult(new { data = new List<TopicViewModel>() });
		}

		[HttpPost("single")]
		public async Task<IActionResult> GetTopic(string topic, string serviceId)
		{
			var topicObj = await _topicService.GetTopicAsync(topic, serviceId);

			return new JsonResult(new
			{
				result = topicObj
			});
		}


		private readonly ILogger _logger;
		private readonly ITopicService _topicService;

	}
}
