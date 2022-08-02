using Microsoft.AspNetCore.Mvc;
using TaskAPI.Core.DTOs;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.Web.Controllers {

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthService _service;

        public AuthController(IAuthService service) {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login) {

            var user = await _service.Authenticate(login);

            if (user != null) {

                var access_token = _service.GenerateAccessToken(user);
                var refresh_token = _service.GenerateRefreshToken(user);

                return Ok(new AuthResponse { AccessToken = access_token, RefreshToken = refresh_token});

            } else {
                return NotFound("Username or password is incorrect!");
            }
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Refresh-token")] string refreshToken) {

            var newToken = await _service.RegenerateAccessToken(refreshToken);

            if (newToken == null) {
                return NotFound("Username not found!");
            }

            return Ok(new AuthResponse { AccessToken = newToken, RefreshToken = refreshToken });
        }
    }
}
