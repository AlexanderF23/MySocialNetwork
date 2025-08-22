namespace MySocialNetwork.Models;

public class ProfileViewModel
{
    public string Username { get; set; }
    public string ProfilePictureUrl { get; set; }
    public List<Post> Posts { get; set; }
}