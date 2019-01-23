using Business.Services;
using DotNetCore.CAP;
using Infrastructure;
using MicroAngels.Bus.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Controllers
{
    //[ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        public MessageController(
            IMessageService messageService,
            ILogger<MessageController> logger,
            CAPMysqlDbContext context,
            ICapPublisher serviceBus,
            IConfiguration configuraton
            )
        {
            _logger = logger;
            _dbContext = context;
            _serviceBus = serviceBus;
            _configuration = configuraton;
            _messageService = messageService;
        }

        //[Route("send")]
        //public async void SendMessage()
        //{
        //    _logger.Log(LogLevel.Warning, "welcome kafka!");

        //    //var role = Request.Headers["Role"];
        //    //var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
        //    //   new AuthTokenRequest(_configuration["IdentityService:Uri"], "messageClient", "secret", "MessageCenter", "alice", "password", null),
        //    //   AuthType.byResoucePassword);



        //    var message = new { Topic = "TestTopic", Body = "Hello kafka", SenderId = "Sender1", SenderTime = "2018-12-01" };
        //    var json = JsonConvert.SerializeObject(message);
        //    await _serviceBus.PublishAsync("mytopic", json);
        //}

        //[CapSubscribe("mytopic", Group = "mygroup")]
        //public async Task<bool> Receive1(string json)
        //{
        //    _logger.Log(LogLevel.Error, "recever 1 consumed!");
        //    return false;
        //}

        //[CapSubscribe("mytopic", Group = "mygroup")]
        //public async Task<bool> Receive2(string json)
        //{
        //    _logger.Log(LogLevel.Error, "recever 2 consumed!");
        //    return false;
        //}

        //[CapSubscribe("mytopic", Group = "mygroup2")]
        //public async Task<bool> Receive3(string json)
        //{
        //    _logger.Log(LogLevel.Error, "recever 3 consumed!");
        //    return false;
        //}


        public string Get()
        {
            return "Hello world v1.0!";
        }

        private readonly ILogger<MessageController> _logger;
        private readonly CAPMysqlDbContext _dbContext;
        private readonly ICapPublisher _serviceBus;
        private readonly IMessageService _messageService;
        private readonly IConfiguration _configuration;
    }
}
