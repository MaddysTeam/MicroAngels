using System.Net.Http;

namespace Common.Auth
{

    public class AuthTokenRequest
    {

        public AuthTokenRequest(string authority, string client, string secret,string api,
            string userName,string password,
            HttpClientHandler httpClientHandler)
        {
            Authority = authority;
            Client = client;
            Secret = secret;
            Api = api;
            Password = password;
            UserName = userName;
            HttpClientHandler = httpClientHandler;
        }

        public string Authority { get; }
        public string Client { get; }
        public string Secret { get; }
        public string Api { get; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public HttpClientHandler HttpClientHandler { get; set; }

    }

    public class AuthTokenResponse
    {
        public AuthTokenResponse(string accessToken, bool isSuccess, string error)
        {
            AccessToken = accessToken;
            IsSuccess = isSuccess;
            Error = error;
        }

        public string AccessToken { get; }
        public bool IsSuccess { get; }
        public string Error { get; }
    }

    public enum AuthType
    {
        byCredential,
        byResoucePassword
    }

}
