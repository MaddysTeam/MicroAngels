using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Models;
using Ocelot.JwtAuthorize;

namespace MyWebService2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly ITokenBuilder _tokenBuilder;

        public LoginController(ITokenBuilder tokenBuilder)
        {
            _tokenBuilder = tokenBuilder;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginModel model)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,"gsw"),
                new Claim(ClaimTypes.Role,"admin")
            };

            var token = _tokenBuilder.BuildJwtToken(claims, DateTime.UtcNow, DateTime.Now.AddSeconds(500000));

            return new JsonResult(new { result = true, data = token });
        }
    }
}
