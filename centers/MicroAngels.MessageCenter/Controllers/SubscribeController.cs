using Business;
using Business.Services;
using MicroAngels.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class SubscribeController : BaseController
	{

		public SubscribeController(ISubscribeService subscribeService)
		{
			_subscribeService = subscribeService;
		}


		[HttpPost("targets")]
		public async Task<IActionResult> GetSubscribeTarget([FromForm]int start, [FromForm] int length)
		{
			var userId = User.GetClaimsValue(CoreKeys.USER_ID);
			var serivceId = User.GetClaimsValue(CoreKeys.SYSTEM_ID);
			var subscribes = await _subscribeService.Search(
				new SubscribeSearchOptions { serviceId = serivceId, subscriberId = userId },
				new PageOptions(start, length)
				);

			return new JsonResult(new
			{
				data = subscribes.Select(r => Mapper.Map<SubscribeViewModel>(r))
			});

		}

		[HttpPost("subscribe")]
		public async Task<IActionResult> Subscribe([FromForm]SubscribeViewModel viewModel)
		{
			var subscribe = Mapper.Map<SubscribeViewModel, Subscribe>(viewModel);
			if (subscribe.IsNull() || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var isSuccess = await _subscribeService.SubscribeAsync(subscribe);

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

	}
}
