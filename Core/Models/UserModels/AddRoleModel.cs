using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos;

public class AddRoleModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public Role.Roles Role { get; set; }
}