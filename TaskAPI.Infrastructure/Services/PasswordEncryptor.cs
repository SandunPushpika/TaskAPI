using TaskAPI.Core.Interfaces;
using crypt = BCrypt.Net.BCrypt;

namespace TaskAPI.Infrastructure.Services {
    public class PasswordEncryptor : IPasswordEncryptor {

        public string HashPassword(string password) {
            
            return crypt.HashPassword(password);
        }

        public Boolean ComparePasswords(string password, string hash) {

            return crypt.Verify(password, hash);
        }
    }
}
