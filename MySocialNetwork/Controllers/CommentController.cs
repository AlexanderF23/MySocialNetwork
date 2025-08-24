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
            return Json(new { success = false, message = "Du skal være logget ind." });


        if (string.IsNullOrEmpty(content))
            return Json(new { succsess = false, message = "Kommentar kan ikke være tom" });

        var comment = new Comment
        {
            Content = content,
            Username = username,
            CreatedAt = DateTime.Now,
            PostId = postId
        };

        _context.Comments.Add(comment);
        _context.SaveChanges();

        return Json(new
        {
            success = true,
            comment = new
            {
                username = comment.Username,
                content = comment.Content,
                createdAt = comment.CreatedAt.ToString("g")
            }
        });
    }
}