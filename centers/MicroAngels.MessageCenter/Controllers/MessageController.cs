using Business.Services;
using DotNetCore.CAP;
using Infrastructure;
using MicroAngels.Bus.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;	

namespace Controllers
{
	/// <summary>
	/// MessageController
	/// </summary>

	//[ApiVersion("1.0")]
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{

		/// <summary>
		/// Structure method
		/// </summary>
		/// <param name="messageService"></param>
		/// <param name="logger"></param>
		/// <param name="context"></param>
		/// <param name="serviceBus"></param>
		/// <param name="configuraton"></param>
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
		[HttpGet("send")]
		[Authorize]
		public string SendMessage()
		{
			throw new NotImplementedException();
		}


		private readonly ILogger<MessageController> _logger;
		private readonly CAPMysqlDbContext _dbContext;
		private readonly ICapPublisher _serviceBus;
		private readonly IMessageService _messageService;
		private readonly IConfiguration _configuration;
	}
}
