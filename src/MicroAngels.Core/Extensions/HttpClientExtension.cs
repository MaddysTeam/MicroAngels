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

	}

}
