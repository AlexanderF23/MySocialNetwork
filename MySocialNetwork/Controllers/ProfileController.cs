using Microsoft.AspNetCore.Mvc;
using MySocialNetwork.Data;
using System.Linq;


namespace MySocialNetwork.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            username = HttpContext.Session.GetString("User");
        }

        var posts = _context.Posts
            .Where(p => p.Username == username)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
        
        ViewBag.Username = username;
        return View(posts);
    }
}