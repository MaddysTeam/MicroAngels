using MicroAngels.Core.Service;
using MicroAngels.GRPC.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroAngels.GRPC.Test.Models
{
	public class GRPCServiceFinder : IGRPCServiceFinder
	{

		public Task<Dictionary<GRPCService, int>> GetServicesAsync(string serviceName)
		{
			throw new NotImplementedException();
		}

	}




}
