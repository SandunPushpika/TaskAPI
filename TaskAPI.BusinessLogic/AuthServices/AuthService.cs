using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Core.DTOs;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.BusinessLogic.AuthServices {
    public class AuthService : IAuthService {

        private IConfiguration _config;
        private IUserService _service;

        public AuthService(IConfiguration config, IUserService service) {
            _config = config;
            _service = service;
        }

        public async Task<UserModel> Authenticate(UserLogin login) {

            var user = await _service.GetUserByUsername(login.Username);

            if (user == null) {
                return null;
            }

            var EnPass = Convert.ToBase64String(Encoding.UTF8.GetBytes(login.Password));

            if (EnPass.Equals(user.Password)) {
                return user;
            }

            return null;
        }

        public string GenerateAccessToken(UserModel model) {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Username", model.Username),
                new Claim(ClaimTypes.Role, model.Role),
                new Claim(ClaimTypes.Name, model.Name)
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
            var securityToken = handler.ReadToken(refresh_token) as JwtSecurityToken;

            var username = securityToken.Claims.First(claim => claim.Type == "Username").Value;

            var user = await _service.GetUserByUsername(username);

            if (user == null) {
                return null;
            }

            return GenerateAccessToken(user);

        }
    }
}
