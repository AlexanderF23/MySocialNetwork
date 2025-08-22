using Microsoft.AspNetCore.Mvc;
using MySocialNetwork.Data;
using System.Linq;
using MySocialNetwork.Models;


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
        
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        
        
        if (user == null)
        {
            return NotFound();
        }

        var posts = _context.Posts
            .Where(p => p.Username == username)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        var vm = new ProfileViewModel()
        {
            Username = user.Username,
            ProfilePictureUrl = string.IsNullOrEmpty(user.ProfilePictureUrl)
                ? "/images/placeholder.png" 
                : user.ProfilePictureUrl,
            Posts = posts
        };
        
        return View(vm);
    }
}