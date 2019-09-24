using MicroAngels.Core.Test.Models;
using System.Net.Http;
using Xunit;

namespace MicroAngels.Core.Test
{

	public class HttpClientExtensionTest
	{

		private HttpClient _client;
		private readonly string _serviceUrl = "http://192.168.1.2:6000/api/getfiles";

		public HttpClientExtensionTest()
		{
			_client = new HttpClient();
		}

		[Fact]
		public async void GetAsyncTest()
		{
			var files = await _client.GetAsync<Files>(_serviceUrl);
			Assert.NotNull(files);
		}

	}

}
