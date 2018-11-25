using Business;
using Common;
using Common.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Discovery;
using System.Threading.Tasks;

namespace Controllers
{

    [Route("Identity")]
    public class IdentityController : Controller
    {

        //public IdentityController(
        //    IConfiguration configuration,
        //    IDiscoveryClient client
        //    )
        //{
        //    _config = configuration;
        //    _handler = new DiscoveryHttpClientHandler(client);
        //}


        //[Route("AccessToken")]
        //public async Task<string> GetAssessTokenByNameAndPassword(string name, string password)
        //{
        //    var client = "client";
        //    var secret = "secret";
        //    var authority = "http://identity";
        //    var api = "TaskApi offline_access";
        //    var accessTokenResponse = await AuthService.RequestAccesstokenAsync(
        //         new AuthTokenRequest(authority, client, secret, api, name, password, _handler),
        //         AuthType.byResoucePassword);
        //    if (accessTokenResponse.IsSuccess)
        //    {
        //        return accessTokenResponse.AccessToken;
        //    }
    
        //    return string.Empty;
        //}


        //private readonly IConfiguration _config;
        //private readonly DiscoveryHttpClientHandler _handler;

    }

}
