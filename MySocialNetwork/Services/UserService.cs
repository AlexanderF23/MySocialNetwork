using MySocialNetwork.Models;
using System.Collections.Generic;
using System.Linq;


namespace MySocialNetwork.Services;

    public class UserService
    {
        private static List<User> _users = new List<User>();

        public bool Register(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
                return false;

            _users.Add(new User { Username = username, Password = password });
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
        }
        
    }