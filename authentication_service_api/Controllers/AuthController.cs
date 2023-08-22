using jwt_auth_manager;
using jwt_auth_manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace authentication_service_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public AuthController(IConfiguration configuration, JwtTokenHandler jwtTokenHandler)
        {
            this._configuration = configuration;
            this._JwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost("sign-in")]
        public ActionResult<AuthRes?> authenticateUser([FromBody] AuthReq user)
        {
            var authRes = this._JwtTokenHandler.GenerateToken(user);
            if (authRes == null) return Unauthorized();
            return authRes;
        }
    }
}
