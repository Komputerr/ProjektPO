using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public class SystemManager
    {
        public Dictionary<string, User> users;
        private SocialMediaPostManager postManager;
        private string usersFilePath = "users.txt";

        public SystemManager()
        {
            users = new Dictionary<string, User>();
            postManager = new SocialMediaPostManager();
        }

        public void Red(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void CreateUsersFile()
        {
            if (!File.Exists(usersFilePath))
            {
                File.Create(usersFilePath).Close();

                File.AppendAllText(usersFilePath, "admin" + ";" + HashPassword("admin") + ";" + "admin" + Environment.NewLine);
                File.AppendAllText(usersFilePath, "guest" + ";" + HashPassword("guest") + ";" + "guest" + Environment.NewLine);
            }
        }

        public void LoadUsers()
        {
            if (File.Exists(usersFilePath))
            {
                string[] lines = File.ReadAllLines(usersFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    var user = new User(parts[0], parts[1], parts[2]);
                    if (!users.ContainsKey(user.Username))
                    {
                        users.Add(user.Username, user);
                    }   
                }
            }
        }

        public User Login(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                var user = users[username];
                if (user.HashedPassword == password)
                {
                    Logger.LogLogin(username, true);
                    Console.WriteLine($"Login successful! Role: {user.Role}");
                    return user;
                }
            }
            Logger.LogLogin(username, false);
            Red("Invalid login or password.");
            return null;
        }

        public void RegisterUser(string username, string password, string role)
        { 
            var user = new User(username, password, role);
            users.Add(username, user);
            File.AppendAllText(usersFilePath, username + ";" + password + ";" + role + Environment.NewLine);
            Console.WriteLine($"User '{username}' registered successfully.");
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
