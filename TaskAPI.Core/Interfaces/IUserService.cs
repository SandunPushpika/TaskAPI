using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Core.Entities;

namespace TaskAPI.Core.Interfaces {
    public interface IUserService {
        public Task<UserModel> AddNewUser(UserModel model);
        public Task<IEnumerable<UserModel>> GetAllUsers();
        public Task<UserModel> GetUserByUsername(string username);
        public Task<UserModel> UpdateUser(UserModel model);
        public Task<UserModel> DeleteUser(int userid);
    }
}
