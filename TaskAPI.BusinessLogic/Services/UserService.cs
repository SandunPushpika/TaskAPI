using Microsoft.Extensions.Logging;
using System.Text;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.BusinessLogic.Repositories {
    public class UserService : IUserService {

        private readonly IDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(IDbContext context, ILogger<UserService> logger) {
            
            _context = context;
            _logger = logger;

        }
        public async Task<UserModel> AddNewUser(UserModel user) {

            var check = await GetUserByUsername(user.Username);

            if (check == null) {

                user.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));

                return await _context.AddObject<UserModel>(
                    "insert into users (Username,Name,Age,Role,Password) values (@Username,@Name,@Age,@Role,@Password)"
                    , user);
            }

            _logger.LogError("{user} username already exists",user.Username);

            return null;

        }

        public async Task<UserModel> DeleteUser(int userid) {

            var user = await _context.GetObjectById<UserModel>("Select * from users where id = @id", userid);

            if (user == null) return null;

            await _context.DeleteById("delete from users where id = @id", userid);

            _logger.LogWarning("{user} deleted",userid);

            return user;

        }

        public Task<IEnumerable<UserModel>> GetAllUsers() {

            return _context.GetAllObjects<UserModel>("select * from users");

        }

        public async Task<UserModel> GetUserByUsername(string username) {

            var user = await _context.GetObjectByUsername<UserModel>(
                "select * from users where username = @username", username);

            return user;
        }

        public async Task<UserModel> UpdateUser(UserModel model) {

            var user = await _context.GetObjectById<UserModel>("Select * from users where Id=@Id",model.Id);

            if (user == null) {
                return null;
            }

            model.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.Password));

            string query = "update users set Username = @Username, Name = @Name, Age = @Age, Role = @Role, Password = @Password where Id = @Id";
            await _context.UpdateObject<UserModel>(query, model);

            _logger.LogInformation("{user} updated",model.Username);
            return model;
        }
    }
}
