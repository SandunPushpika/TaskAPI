using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Core.DTOs;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.BusinessLogic.AuthServices {
    public class AuthService : IAuthService {

        private readonly IConfiguration _config;
        private readonly IUserService _service;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordEncryptor _passwordEncryptor;

        public AuthService(IConfiguration config, IUserService service, ILogger<AuthService> logger, IPasswordEncryptor passwordEncryptor) {
            _config = config;
            _service = service;
            _logger = logger;
            _passwordEncryptor = passwordEncryptor;
        }

        public async Task<UserModel> Authenticate(UserLogin login) {

            var user = await _service.GetUserByUsername(login.Username);

            if (user == null) {
                _logger.LogInformation("User is null here!");
                return null;
            }

            if (_passwordEncryptor.ComparePasswords(login.Password, user.Password)) {
                _logger.LogInformation("Password matches");
                return user;
            }

            _logger.LogInformation("Password does not match!");
            return null;
        }

        public string GenerateAccessToken(UserModel model) {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Username", model.Username),
                new Claim(ClaimTypes.Role, model.Role),
                new Claim(ClaimTypes.Name, model.Name),
                new Claim("Age", model.Age.ToString()),
            };

            var access_token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credintials
                );

            return new JwtSecurityTokenHandler().WriteToken(access_token);

        }

        public string GenerateRefreshToken(UserModel model) {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Username", model.Username)
            };

            var refresh_token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: credintials
                );

            return new JwtSecurityTokenHandler().WriteToken(refresh_token);

        }

        public async Task<string> RegenerateAccessToken(string refresh_token) {

            var handler = new JwtSecurityTokenHandler();
            try {

                var securityToken = handler.ReadToken(refresh_token) as JwtSecurityToken;

                var username = securityToken.Claims.First(claim => claim.Type == "Username").Value;

                var user = await _service.GetUserByUsername(username);

                if (user == null) {
                    return null;
                }

                return GenerateAccessToken(user);

            } catch (Exception ex) {
                
                _logger.LogError(ex, "Invalid Token Passed to RegenerateAccessToken(string refresh_token)");
                return "";
            } 

        }

        public LoggedUser GetLoggedUser(string acces_token) {

            var handler = new JwtSecurityTokenHandler();
            try {

                var token = handler.ReadToken(acces_token) as JwtSecurityToken;

                var username = token.Claims.First(claim => claim.Type == "Username").Value;
                var role = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                return new LoggedUser { Username = username, Role = role };

            } catch (Exception ex) {
                _logger.LogError(ex, "Invalid Jwt Token");
                return null;
            }

        }
    }
}
