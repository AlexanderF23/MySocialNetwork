namespace MySocialNetwork.Models;

public class Like
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    
    public string Username { get; set; }
}