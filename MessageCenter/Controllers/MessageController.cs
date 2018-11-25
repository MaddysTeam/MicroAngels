//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using DotNetCore.CAP;
//using zipkin4net.Transport.Http;
//using Business;
//using Infrastructure;

//namespace Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MessageController : ControllerBase
//    {

//        public MessageController(
//            ILogger<MessageController> logger,
//            CAPMysqlDbContext context,
//            ICapPublisher serviceBus
//            )
//        {
//            _logger = logger;
//            _dbContext = context;
//            _serviceBus = serviceBus;
//        }

//        [Route("send")]
//        public void SendMessage()
//        {
//            PublishMessage("hello kafka");
//        }

//        protected void PublishMessage(string message)
//        {
//            using (var trans = _dbContext.Database.BeginTransaction())
//            {
//                _serviceBus.Publish("dgp", message);
//            }
//        }

//        [CapSubscribe("dgp")]
//        public void ReceiveMessage(string message)
//        {
//            //return new ServerOkResult("ok");
//        }

//        private readonly ILogger<MessageController> _logger;
//        private readonly CAPMysqlDbContext _dbContext;
//        private readonly ICapPublisher _serviceBus;

//    }
//}
