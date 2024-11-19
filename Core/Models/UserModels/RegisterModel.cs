using System.ComponentModel.DataAnnotations;

namespace Core.Dtos;

public class RegisterModel
{
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}