using Microsoft.Extensions.Logging;
using System.Text;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Helpers;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.BusinessLogic.Repositories {
    public class UserService : IUserService {

        private readonly IDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordEncryptor _encryptor;

        public UserService(IDbContext context, ILogger<UserService> logger, IPasswordEncryptor encryptor) {

            _context = context;
            _logger = logger;
            _encryptor = encryptor;

        }
        public async Task<UserModel> AddNewUser(UserModel user) {

            var check = await GetUserByUsername(user.Username);

            if (check == null) {

                user.Password = _encryptor.HashPassword(user.Password);

                string query = new FluentQueryBuilder()
                    .InsertInto("users")
                    .Add(true ,"Username" ,"Name" ,"Age" ,"Role" ,"Password")
                    .Values("@Username", "@Name", "@Age", "@Role", "@Password")
                    .Build();

                _logger.LogInformation("Query: {query}",query);

                return await _context.AddObject<UserModel>(query ,user);
            }

            _logger.LogError("{user} username already exists",user.Username);

            return null;

        }

        public async Task<UserModel> DeleteUser(int userid) {

            string selectQuery = new FluentQueryBuilder()
                .SelectAll()
                .From("users")
                .Where("id","@id","=")
                .Build();
            var user = await _context.GetObjectById<UserModel>(selectQuery, userid);
            
            if (user == null) return null;

            string deleteQuery = new FluentQueryBuilder()
                .Delete()
                .From("users")
                .Where("id", "@id", "=")
                .Build();

            await _context.DeleteById(deleteQuery, userid);
            _logger.LogWarning("{user} deleted",userid);

            return user;

        }

        public Task<IEnumerable<UserModel>> GetAllUsers() {

            return _context.GetAllObjects<UserModel>(new FluentQueryBuilder().SelectAll().From("users").Build());

        }

        public async Task<UserModel> GetUserByUsername(string username) {

            var user = await _context.GetObjectByUsername<UserModel>(
                "select * from users where username = @username", username);

            return user;
        }

        public async Task<UserModel> UpdateUser(UserModel model) {

            string selectQuery = new FluentQueryBuilder()
                .SelectAll()
                .From("users")
                .Where("id", "@id", "=")
                .Build();

            var user = await _context.GetObjectById<UserModel>(selectQuery, model.Id);

            if (user == null) {
                return null;
            }

            model.Password = _encryptor.HashPassword(model.Password);

            string updateQuery = new FluentQueryBuilder()
                .Update("users")
                .Add(false, "Username = @Username", "Name = @Name", "Age = @Age", "Role = @Role", "Password = @Password")
                .Where("ID", "@Id", "=")
                .Build();
        
            await _context.UpdateObject<UserModel>(updateQuery, model);

            _logger.LogInformation("{user} updated",model.Username);
            return model;
        }
    }
}
