using Microsoft.AspNetCore.Mvc;
using MySocialNetwork.Data;
using MySocialNetwork.Models;

namespace MySocialNetwork.Controllers;

public class PostsController : Controller
{ 
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var posts = _context.Posts.ToList();
        return View(posts);
    }

    [HttpPost]
    public IActionResult Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError(string.Empty, "Opslag kan ikke være tomt");
            return RedirectToAction("Index");
        }

        var username = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login", "Account");
        

        var post = new Post
        {
            Content = content,
            Username = username,
            CreatedAt = DateTime.Now
        };
        
        _context.Posts.Add(post);
        _context.SaveChanges();
        
        return RedirectToAction("Index");
    }
}