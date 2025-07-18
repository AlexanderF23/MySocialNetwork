
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MySocialNetwork.Controllers
{
    public class AccountController : Controller
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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
        public IActionResult Register()
        {
            return View();
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