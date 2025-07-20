using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySocialNetwork.Data;
using MySocialNetwork.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;


namespace MySocialNetwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        
        //get /Account/Register
        public IActionResult Register()
        {
            return View();
        }
        
        //Post /Account/Register
        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            //tjek om brugernavn findes
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(string.Empty, "Brugernavnet er allerede i brug");
                return View(model);
            }

            var user = new User { Username = model.Username };
            user.Password = _passwordHasher.HashPassword(user, model.Password); //Hashing af adgangskode
            
            
            _context.Users.Add(user);
            _context.SaveChanges();
            
            HttpContext.Session.SetString("User", user.Username);
            return RedirectToAction("Index", "Home");
        }
        
        //get /Account/Login
        public IActionResult Login()
        {
            return View();
        }
        
        //post /Account/Login
        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("User", user.Username);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            ModelState.AddModelError(string.Empty, "Forkert brugernavn eller adgangskode.");
            return View(model);
        }
        
        //get /Account/logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}























//OLD AccountController
/*
namespace MySocialNetwork.Controllers
{
    public class AccountController : Controller
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();

        [HttpGet]
        public IActionResult Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
            return View(model);

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Udfyld både brugernavn og adgangskode.");
                return View(model);
            }

            if (users.TryGetValue(model.Username, out var correctPassword) && correctPassword == model.Password)
            {
                HttpContext.Session.SetString("User", model.Username);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError("", "Forkert brugernavn eller adgangskode");
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (users.TryGetValue(username, out var correctPassword) && correctPassword == password)
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Forkert brugernavn eller adgangskode.";
            return View();
        }

        [HttpGet]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            return View(model);

            if (users.ContainsKey(model.Username))
            {
                ModelState.AddModelError("Username", "Brugernavn findes allerede");
                return View(model);
            }
            
            users[model.Username] = model.Password;
            HttpContext.Session.SetString("User", model.Username);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (!users.ContainsKey(username))
            {
                users[username] = password;
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Brugernavnet findes allerede.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }

        
    }
}
*/