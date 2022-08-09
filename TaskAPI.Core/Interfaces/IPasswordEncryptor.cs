using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.Core.Interfaces {
    public interface IPasswordEncryptor {

        public string HashPassword(string password);
        public Boolean ComparePasswords(string password, string hash);

    }
}
