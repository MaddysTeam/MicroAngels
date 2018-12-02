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
using Newtonsoft.Json;
using Common.Auth;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        public MessageController(
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
        }

        [Route("send")]
        public async void SendMessage()
        {

            //var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
            //   new AuthTokenRequest(_configuration["IdentityService:Uri"], "messageClient", "secret", "MessageCenter", "account", "password", null),
            //   AuthType.byResoucePassword);


            
            //var message = new { Topic = "TestTopic", Body = "Hello kafka", SenderId = "Sender1", SenderTime = "2018-12-01" };
            //var json = JsonConvert.SerializeObject(message);
            //PublishMessage(json);
        }

        //protected  void PublishMessage(string message)
        //{
        //    using (var trans = _dbContext.Database.BeginTransaction())
        //    {
        //       _serviceBus.Publish("MessageCenter.Subscribe", message);
        //    }
        //}

        //[CapSubscribe("dgp")]
        //public async  Task ReceiveMessage(string message)
        //{
        //    //return new ServerOkResult("ok");
        //}

        private readonly ILogger<MessageController> _logger;
        private readonly CAPMysqlDbContext _dbContext;
        private readonly ICapPublisher _serviceBus;
        private readonly IMessageService _messageService;
        private readonly IConfiguration _configuration;
    }
}
