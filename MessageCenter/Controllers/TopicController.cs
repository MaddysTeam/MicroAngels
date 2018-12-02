using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.CAP;
using zipkin4net.Transport.Http;
using Business;
using Infrastructure;
using Business.Services;

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


        private readonly ILogger<MessageController> _logger;
        private readonly ITopicService _topicService;

    }
}
