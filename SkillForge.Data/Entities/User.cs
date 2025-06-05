using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(Username), nameof(PasswordHash))]
public class User {
    [Key]
    public int ID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    //public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }   
}