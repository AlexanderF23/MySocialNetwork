using Microsoft.AspNetCore.Mvc;
using MySocialNetwork.Data;
using MySocialNetwork.Models;


namespace MySocialNetwork.Controllers;

public class CommentController : Controller
{
    private readonly AppDbContext _context;

    public CommentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create(int postId, string content)
    {
        var username = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("login", "Account");
        
        if (string.IsNullOrEmpty(content))
            return RedirectToAction("Index", "Posts");

        var comment = new Comment
        {
            Content = content,
            Username = username,
            CreatedAt = DateTime.Now,
            PostId = postId
        };

        _context.Comments.Add(comment);
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Posts");
    }
}