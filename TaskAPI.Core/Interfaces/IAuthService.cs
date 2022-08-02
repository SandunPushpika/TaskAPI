using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Core.DTOs;
using TaskAPI.Core.Entities;

namespace TaskAPI.Core.Interfaces {
    public interface IAuthService {
        public Task<UserModel> Authenticate(UserLogin login);
        public string GenerateAccessToken(UserModel model);
        public string GenerateRefreshToken(UserModel model);
        public Task<string> RegenerateAccessToken(string refresh_token);
    }
}
