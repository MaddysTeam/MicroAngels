using Business;
using Business.Helpers;
using Business.Services;
using DotNetCore.CAP;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
	/// <summary>
	/// MessageController
	/// </summary>

	//[ApiVersion("1.0")]
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : BaseController
	{

		/// <summary>
		/// Structure method
		/// </summary>
		/// <param name="messageService"></param>
		/// <param name="logger"></param>
		/// <param name="context"></param>
		/// <param name="serviceBus"></param>
		/// <param name="configuraton"></param>
		public MessageController(IMessageService messageService) : base()
		{
			_messageService = messageService;
		}


		[HttpPost("sendAnnounce")]
		public async Task<IActionResult> SendAnnounceMessage([FromForm] MessageViewModel messageViewModel)
		{
			var message = Mapper.Map<MessageViewModel, Message>(messageViewModel);
			message.SenderId = User.GetClaimsValue(CoreKeys.USER_ID);
			var isSuccess = await _messageService.AnnounceAsync(message);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "发送成功" : "发送失败"
			});
		}

		[HttpPost("announces")]
		public async Task<IActionResult> GetAnnounceMessage([FromForm] string topicId, [FromForm]string serviceId, [FromForm]int start, [FromForm] int length)
		{
			var totalCount = 0;
			var searchResults = await _messageService.Search(topicId, serviceId, StaticKeys.MessageTypeId_Announce, length, start, out totalCount);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(m => Mapper.Map<Message, MessageViewModel>(m)),
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


		private readonly IMessageService _messageService;

	}

}
