using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace MicroAngels.Core
{

	public static class HttpClientExtension
	{

		public static async Task<T> GetAsync<T>(this HttpClient httpClient, string serviceUrl, bool ensureSuccess = false)
		{
			if (serviceUrl.IsNullOrEmpty()) return default(T);

			var data = await httpClient.GetAsync(serviceUrl);
			if (!data.Content.IsNull())
				return await data.Content.ReadAsObjectAsync<T>(ensureSuccess);

			return default(T);
		}


		public static async Task<T> PostAsync<T>(this HttpClient httpClient, string serviceUrl, object body = null, bool ensureSuccess = false)
		{
			if (serviceUrl.IsNullOrEmpty()) return default(T);

			var data = await httpClient.PostAsJsonAsync(serviceUrl, body);
			if (!data.Content.IsNull())
				return await data.Content.ReadAsObjectAsync<T>(ensureSuccess);

			return default(T);
		}


		public static async Task<T> PostAsync<T, S>(
			this HttpClient httpClient,
			string serviceName,
			string virtualPath,
			object body = null,
			IServiceFinder<S> serviceFinder = null,
			ILoadBalancer balancer=null) where S : IService
		{
			if (serviceName.IsNullOrEmpty())
				throw new AngleExceptions("service name not exits");

			if (virtualPath.IsNullOrEmpty())
				throw new AngleExceptions("virtualPath not exits");

			if (serviceFinder.IsNull())
				throw new AngleExceptions("serviceFinder cannot be null");

			balancer = balancer ?? new WeightRoundBalancer();

			var userServices = await serviceFinder.FindByNameAsync(serviceName);
			var foundedService = default(S);
			if (!userServices.IsNull() && userServices.Count > 0)
			{
				var servicesDictionary = (from us in userServices select new { service = us, weight = us.Weight, }).ToDictionary(x => x.service, y => y.weight);
				foundedService = balancer.Balance(servicesDictionary);
			}

			using (var client = new HttpClient())
			{
				var url = foundedService?.Address.ToString() + @"\" + virtualPath;
				return await client.PostAsync<T>(url);
			}
		}

	}

}
