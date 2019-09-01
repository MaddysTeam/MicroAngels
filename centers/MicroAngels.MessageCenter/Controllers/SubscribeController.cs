using Business;
using Business.Helpers;
using Business.Services;
using MicroAngels.Bus.CAP;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class SubscribeController : BaseController
	{

		public SubscribeController(ISubscribeService subscribeService, ICAPPublisher publisher)
		{
			_subscribeService = subscribeService;
			_publisher = publisher;
		}


		[HttpPost("targets")]
		public async Task<List<SubscribeViewModel>> GetSubscribeTarget([FromBody]SubscribeViewModel viewModel)
		{
			var userId = viewModel?.SubscriberId ?? User.GetClaimsValue(CoreKeys.USER_ID);
			var serivceId = viewModel?.ServiceId ?? User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var subscribes = await _subscribeService.Search(
				new SubscribeSearchOptions { serviceId = serivceId, subscriberId = userId },
				null
				);

			List<SubscribeViewModel> results = new List<SubscribeViewModel>();
			foreach (var item in subscribes)
			{
				results.Add(Mapper.Map<SubscribeViewModel>(item));
			}

			return results;
		}



		[HttpPost("subscribe")]
		public async Task<IActionResult> Subscribe([FromForm]SubscribeViewModel viewModel)
		{
			viewModel = viewModel ?? new SubscribeViewModel();
			viewModel.ServiceId = CurrnetUser.ServiceId;
			viewModel.SubscriberId = CurrnetUser.UserId;

			var subscribe = Mapper.Map<SubscribeViewModel, Subscribe>(viewModel);
			if (subscribe.IsNull() || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var isSuccess = await _subscribeService.SubscribeAsync(subscribe);

			if (isSuccess)
				await _publisher.PublishAsync(new Message { Topic=AppKeys.MessageCenterNotfiy, ServiceId=CurrnetUser.ServiceId, TopicId=viewModel.TopicId, Body="", HasTrans=false,SendTime=DateTime.UtcNow });

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}


		[HttpPost("unSubscribe")]
		public async Task<IActionResult> UnSubscribe([FromForm]SubscribeViewModel viewModel)
		{
			var subscribe = Mapper.Map<SubscribeViewModel, Subscribe>(viewModel);
			if (subscribe.IsNull() || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var isSuccess = await _subscribeService.UnSubsribeAsync(subscribe);

			return new JsonResult(new
			{
				isSuccess,
				msg = isSuccess ? "操作成功" : "操作失败"
			});
		}


		private ISubscribeService _subscribeService;
		private ICAPPublisher _publisher;
	}
}
