
using IdentityModel.Client;
using System.Threading.Tasks;

namespace Common.Auth
{

    public static class AuthService
    {
        /// <summary>
        /// get access token from identity server
        /// </summary>
        /// <param name="request">AuthTokenRequest</param>
        /// <returns>AuthTokenResponse</returns>
        public static async Task<AuthTokenResponse> RequestAccesstokenAsync(AuthTokenRequest request, AuthType authType)
        {
            AuthTokenResponse authTokenResponse;
            var clientHandler = request.HttpClientHandler;
            var client = new DiscoveryClient(request.Authority, clientHandler)
            {
                Policy = new DiscoveryPolicy { RequireHttps = false }
            };

            var response = await client.GetAsync();
            if (response.IsError)
            {
                return authTokenResponse = new AuthTokenResponse("", false, response.Error);
            }

            TokenResponse tokenResponse;
            var tokenClient = new TokenClient(response.TokenEndpoint, request.Client, request.Secret, clientHandler);
            if (authType == AuthType.byCredential)
                tokenResponse = await tokenClient.RequestClientCredentialsAsync(request.Api);
            else
                tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(request.UserName, request.Password, request.Api);

            if (tokenResponse.IsError)
            {
                authTokenResponse = new AuthTokenResponse("", tokenResponse.IsError, tokenResponse.Error);
            }
            return new AuthTokenResponse(tokenResponse.AccessToken, true, null);
        }

    }

}
