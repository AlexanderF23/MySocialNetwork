
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySocialNetwork.Models;

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