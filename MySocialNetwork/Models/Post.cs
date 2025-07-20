namespace MySocialNetwork.Models;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; } // brugernavn på post
    public DateTime CreatedAt { get; set; }
}