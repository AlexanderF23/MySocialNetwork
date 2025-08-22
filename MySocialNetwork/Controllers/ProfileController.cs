using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySocialNetwork.Data;
using System.Linq;
using MySocialNetwork.Models;
using System.IO;


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

    [HttpPost]
    public IActionResult UploadProfilePicture(IFormFile profilePicture)
    {
        var username = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("login", "Account");
        
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
            return NotFound();

        if (profilePicture != null && profilePicture.Length > 0)
        {
            //gemmer billedet i root/images. skal laves om!
            var fileName = $"{username}_{DateTime.Now.Ticks}{Path.GetExtension(profilePicture.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profilePicture.CopyTo(stream);
            }
            
            //opdatere database url
            user.ProfilePictureUrl = $"/images/profiles/{fileName}";
            _context.SaveChanges();
        }
        
        return RedirectToAction("Index");
    }
}