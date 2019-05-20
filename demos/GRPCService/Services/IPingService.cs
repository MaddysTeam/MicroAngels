using GRPCService.Models;
using MagicOnion;
using MagicOnion.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPCService.Services
{

	public interface IPingService:IService<IPingService>
	{
		UnaryResult<Pong> Ping(Ping ping);
	}

	public class PingService : ServiceBase<IPingService>, IPingService
	{
		public async UnaryResult<Pong> Ping(Ping ping) => new Pong { Message = ping.Message };
	}

}
