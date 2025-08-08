using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
    var posts = _context.Posts
        .Select(p => new PostViewModel
        {
            Id = p.Id,
            Content = p.Content,
            Username = p.Username,
            CreatedAt = p.CreatedAt,
            Likes = _context.Likes.Count(l => l.PostId == p.Id)  // tæller likes
        })
        .ToList();

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
    
    //Get:/Post/Edit/5
    public IActionResult Edit(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
            return NotFound();
        
        var username = HttpContext.Session.GetString("User");
        if (post.Username != username)
            return Forbid(); //må ikke redigere andres opslag

        return View(post);
    }
    
    //POST /Pots/edit/5
    [HttpPost]
    public IActionResult Edit(int id, string content)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
            return NotFound();
        
        var username = HttpContext.Session.GetString("User");
        if (post.Username != username)
            return Forbid();

        if (string.IsNullOrEmpty(content))
        {
            ModelState.AddModelError(string.Empty, "Indhold kan ikke være tomt");
            return View(post);
        }
        
        post.Content = content;
        _context.SaveChanges();
        
        TempData["Message"] = "Opdateret";
        
        
        return RedirectToAction("Index");
    }
    
    //POST /post/delet/5
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
            return NotFound();
        
        var username = HttpContext.Session.GetString("User");
        if (post.Username != username)
            return Forbid();

        _context.Posts.Remove(post);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Like(int id)
    {
        var username = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var like = _context.Likes.FirstOrDefault(l => l.PostId == id && l.Username == username);

        if (like == null)
        {
            // Like
            _context.Likes.Add(new Like { PostId = id, Username = username });
            _context.SaveChanges();
        }
        else
        {
            // Unlike
            _context.Likes.Remove(like);
            _context.SaveChanges();
        }

        int likeCount = _context.Likes.Count(l => l.PostId == id);

        return Json(new { likes = likeCount });
    }


    [HttpPost]
    public IActionResult ToggleLike(int id) 
    {
        var username = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(username))
            return Unauthorized(new { message = "Du skal være logget ind for at like." });

        var like = _context.Likes.FirstOrDefault(l => l.PostId == id && l.Username == username);
        if (like != null)
        {
            // Brugeren allerede liket, så fjern like (unlike)
            _context.Likes.Remove(like);
        }
        else
        {
            // Tilføj like
            _context.Likes.Add(new Like { PostId = id, Username = username });
        }
        _context.SaveChanges();

        // Returnér det nye antal likes 
        var likeCount = _context.Likes.Count(l => l.PostId == id);
        return Json(new { likes = likeCount });
    }
}