using MySocialNetwork.Models;

namespace MySocialNetwork.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
}