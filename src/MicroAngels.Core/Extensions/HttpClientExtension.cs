using MicroAngels.Core.Plugins;
using MicroAngels.Core.Service;
using System.Net.Http;
using System.Threading.Tasks;

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


		public static async Task<T> PostAsync<T,S>(this HttpClient httpClient, string serviceName, IServiceFinder<S> serviceFinder,ILoadBalancer balancer) where S:IService
		{
			if (string.IsNullOrEmpty(serviceName))
				throw new AngleExceptions("service name not exits");

			if (serviceFinder.IsNull())
				throw new AngleExceptions("serviceFinder cannot be null");

			balancer = balancer ?? new WeightRoundBalancer();

			var userServices = await serviceFinder.FindByNameAsync(serviceName);
			var userService = userServices[0];

			using (var client = new HttpClient())
			{
				var url = string.Empty;
				return await client.PostAsync<T>(url);
			}
		}

	}

}
