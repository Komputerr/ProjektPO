using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }

        public User(string username, string hashedPassword, string role)
        {
            Username = username;
            HashedPassword = hashedPassword;
            Role = role;
        }
    }
}
