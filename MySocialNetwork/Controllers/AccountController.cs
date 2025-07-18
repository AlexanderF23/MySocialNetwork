using Microsoft.AspNetCore.Mvc;
using MySocialNetwork.Services;


namespace MySocialNetwork.Controllers;

public class AccountController : Controller
{
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (UserService.ValidateLogin(username, password))
        {
            HttpContext.Session.SetString("User", username);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Forkert Brugernavn eller adgangskode";
        return View();
    }


    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (UserService.Register(username, password))
            return RedirectToAction("Login");

        ViewBag.Error = "Brugernavn eksisterer allerede";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("User");
        return RedirectToAction("Login");
    }

}