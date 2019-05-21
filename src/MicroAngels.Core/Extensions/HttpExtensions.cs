using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MicroAngels.Core
{

	public static class HttpResponseExtensions
	{

		public static async Task SendServerErrorResponse(this HttpResponse response, string contentType, string error)
		{
			response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
			response.ContentType = contentType;
			await response.WriteAsync(error);
		}

		public static async Task SendOKResponse(this HttpResponse response, string contentType, string message)
		{
			response.StatusCode = (int)System.Net.HttpStatusCode.OK;
			response.ContentType = contentType;
			await response.WriteAsync(message);
		}

	}

}
