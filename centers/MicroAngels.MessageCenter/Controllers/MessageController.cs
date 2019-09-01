using Business;
using Business.Helpers;
using Business.Services;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
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
		public MessageController(IMessageService messageService, ISubscribeService subscribeService) : base()
		{
			_messageService = messageService;
			_subscribeService = subscribeService;
		}


		//Post  Send announce message

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
		public async Task<IActionResult> GetAnnounceMessage([FromForm]int start, [FromForm]int length)
		{
			var systemId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var page = new PageOptions(start, length);
			var searchResults =await _messageService.Search(new MessageSearchOptions { serviceId = systemId, typeId = StaticKeys.MessageTypeId_Announce }, page);
			if (!searchResults.IsNull() && searchResults.Count() > 0)
			{
				return new JsonResult(new
				{
					data = searchResults.Select(m => Mapper.Map<Message, MessageViewModel>(m)),
					recordsTotal = page.TotalCount,
					recordsFiltered = page.TotalCount,
				});
			}

			return new JsonResult(new
			{
				data = new { },
				recordsTotal = 0,
				recordsFiltered = 0,
			});
		}


		[HttpPost("unreadAnnounces")]
		public async Task<IActionResult> UnRreadAnnounceMessage()
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var serviceId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var searchOptions = new MessageSearchOptions { serviceId = serviceId, typeId = StaticKeys.MessageTypeId_Announce, reveiverId = userId };
			var result = await _messageService.GetUnReadMessage(searchOptions);

			return new JsonResult(new
			{
				data = result.Select(m => Mapper.Map<UserMessage, UserMessageViewModel>(m))
			});
		}

		[HttpPost("receiveAnnounce")]
		public async Task<IActionResult> ReceiveAnnounce()
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var serviceId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var searchOptions = new MessageSearchOptions { reveiverId = userId, serviceId = serviceId, typeId = StaticKeys.MessageTypeId_Announce };
			var isSuccess = await _messageService.ReceiveMessages(searchOptions);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		[HttpPost("targetMessages")]
		public async Task<IActionResult> GetTargetMessages([FromForm] string topicId, [FromForm]int start, [FromForm]int length)
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var serviceId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			// get all subscribe target id 
			var targetIds = await _subscribeService.Search(new SubscribeSearchOptions { subscriberId = userId }, null);
			var searchOptions = new MessageSearchOptions
			{
				reveiverId = userId,
				serviceId = serviceId,
				topicId = topicId,
				typeId=StaticKeys.MessageTypeId_Notify,
				senderIds = targetIds.IsNull() || targetIds.Count <= 0 ? new string[] { } : targetIds.Select(t => t.TargetId).ToArray()
			};
			var result = await _messageService.SearchUserMessage(searchOptions, new PageOptions(start, length));

			return new JsonResult(new
			{
				data = result.Select(m => Mapper.Map<UserMessage, UserMessageViewModel>(m))
			});
		}

		[HttpPost("receiveTargetMessages")]
		public async Task<IActionResult> ReceiveTargetMessages([FromForm]string topicId)
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var serviceId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var targetIds = await _subscribeService.Search(new SubscribeSearchOptions { subscriberId = userId }, null);
			var searchOptions = new MessageSearchOptions
			{
				reveiverId = userId,
				serviceId = serviceId,
				topicId = topicId,
				typeId  = StaticKeys.MessageTypeId_Notify,
				senderIds = targetIds.IsNull() || targetIds.Count <= 0 ? new string[] { } : targetIds.Select(t => t.TargetId).ToArray()
			};

			var isSuccess = await _messageService.ReceiveMessages(searchOptions);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}

		private readonly IMessageService _messageService;
		private readonly ISubscribeService _subscribeService;

	}

}
