using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Users;

public class UserRegister
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
}