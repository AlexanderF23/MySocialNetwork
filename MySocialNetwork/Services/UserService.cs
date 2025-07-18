using MySocialNetwork.Models;
using System.Collections.Generic;
using System.Linq;


namespace MySocialNetwork.Services;

    public class UserService
    {
        private static List<User> Users = new();

        public static bool Register(string username, string password)
        {
            if (Users.Any(u => u.Username == username)) return false;

            Users.add(new User
            {
                Username = username,
                Password = password
            });

            return true;
        }

        public static bool ValidateLogin(string username, string password)
        {
            return Users.Any(u => u.Username == username && u.Password == password);
        }
    


    }