using DotNetCore.CAP;
using MicroAngels.Core;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace MicroAngels.Bus.CAP
{

	public interface ICAPPublisher: IPublisher
	{

	}

	public class CAPMysqlPublisher : ICAPPublisher
	{
		public CAPMysqlPublisher(ICapPublisher inner, CAPService capService)
		{
			_inner = inner;
			_capService = capService;
		}

		public async Task<Message> PublishAsync<Message>(Message msg) where Message : IMessage
		{
			msg.EnsureNotNull(()=> new MicroAngels.Core.AngleExceptions("message instance cannot be null"));

			if (msg.HasTrans)
			{
				using (var connection = new MySqlConnection(_capService.ConnectString))
				{
					await _inner.PublishAsync(msg.Topic, msg);
				}
			}
			else
			{
				await _inner.PublishAsync(msg.Topic, msg);
			}

			return msg;
		}


		private readonly ICapPublisher _inner;
		private readonly CAPService _capService;

	}

}
