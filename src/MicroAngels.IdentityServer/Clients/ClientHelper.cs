using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using MicroAngels.IdentityServer.Models;

namespace MicroAngels.IdentityServer.Clients
{

	public class ClientHelper
	{

		/// <summary>
		/// get token response
		/// </summary>
		/// <param name="request">token reqeust</param>
		/// <param name="requestType">requestType</param>
		/// <returns>AngelTokenResponse</returns>
		public static async Task<AngelTokenResponse> GetTokenResponse(AngelTokenRequest request, TokenRequestType requestType)
		{
			if (request == null) return null;

			AngelTokenResponse angelResposne = null;
			TokenResponse response = null;
			TokenRevocationResponse revocationResponse = null;

			switch (requestType)
			{
				case TokenRequestType.client_credential:
					response = await GetClientCredentialToken(request.Map()); 
					angelResposne = response?.Map();
					break;
				case TokenRequestType.resource_password:
					response = await GetResourcePasswordToken(new PasswordTokenRequest
					{

					}); break;
				case TokenRequestType.revocation:
					revocationResponse = await ForceTokenTimeout(new TokenRevocationRequest
					{

					}); break;

				case TokenRequestType.refresh:
					response = await ReferenceToken(request.MapRefRequest());
					angelResposne = response?.Map();
					break;
			}

			return angelResposne;

		}


		private static async Task<TokenResponse> GetClientCredentialToken(ClientCredentialsTokenRequest request)
		{
			TokenResponse response = null;
			if (request != null)
				response = await new HttpClient().RequestClientCredentialsTokenAsync(request);

			return response;
		}

		private static async Task<TokenResponse> GetResourcePasswordToken(PasswordTokenRequest request)
		{
			TokenResponse response = null;
			if (request != null)
				response = await new HttpClient().RequestPasswordTokenAsync(request);

			return response;
		}

		private static async Task<TokenRevocationResponse> ForceTokenTimeout(TokenRevocationRequest request)
		{
			TokenRevocationResponse response = null;
			if (request != null)
				response = await new HttpClient().RevokeTokenAsync(request);

			return response;
		}

		private static async Task<TokenResponse> ReferenceToken(RefreshTokenRequest request)
		{
			TokenResponse response = null;
			if (request != null)
				response = await new HttpClient().RequestRefreshTokenAsync(request);

			return response;
		}

	}

}
