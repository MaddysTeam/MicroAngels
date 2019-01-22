using Business.Helpers;
using Business.Models;
using Business.Services;
using MicroAngels.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {

        public TopicController(
            ILogger<MessageController> logger,
            ITopicService topicService
            )
        {
            _logger = logger;
            _topicService = topicService;
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> EditTopic([FromBody] Topic topic)
        {
            if (topic.IsNull() || !topic.IsValidate || !ModelState.IsValid)
            {
                _logger.LogError(AlterKeys.Error.TOPIC_INVALID);

                return BadRequest(ModelState);
            }

            var result = await _topicService.EditTopicAsync(topic);
            if (result)
                return Ok(result);
            else
                return NotFound();
        }

        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> GetTopics(string topic, int pageSize, int pageIndex)
        {
            var totalCount = 0;
            var topics = await _topicService.GetTopicsAsync(topic, pageSize, pageIndex, out totalCount);

            return new JsonResult(new {
                totalCount,
                rows = topics
            });
        }

        [Route("single")]
        [HttpPost]
        public async Task<IActionResult> GetTopic(string topic, string serviceId)
        {
            var topicObj = await _topicService.GetTopicAsync(topic, serviceId);

            return new JsonResult(new
            {
                result = topicObj
            });
        }


        private readonly ILogger<MessageController> _logger;
        private readonly ITopicService _topicService;

    }
}
