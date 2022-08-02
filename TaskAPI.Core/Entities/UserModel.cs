using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.Core.Entities {
    public class UserModel {
        public string Username { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public string Role { get; set; }
        public string Password { get; set; }
    }
}
