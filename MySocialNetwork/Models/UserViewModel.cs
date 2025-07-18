using System.ComponentModel.DataAnnotations;


namespace MySocialNetwork.Models;

public class UserViewModel
{
    [Required(ErrorMessage = "Brugernavn er påkrævet")]
    [MinLength(3, ErrorMessage = "Brugernavn skal være mindst 3 tegn")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Adgangskode er påkærvet")]
    [MinLength(5, ErrorMessage = "Adgangskode skal mindst 5 tegn")]
    public string Password { get; set; } 
}