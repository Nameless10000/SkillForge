using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Users;

public class UserLogin
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}