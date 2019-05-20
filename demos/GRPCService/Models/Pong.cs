using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRPCService.Models
{
	[MessagePackObject]
	public class Pong
	{
		[Key(0)]
		public string Message { get; set; }
	}

}
