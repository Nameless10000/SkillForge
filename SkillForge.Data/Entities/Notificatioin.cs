using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(IsRead))]
[Index(nameof(CreatedAt), AllDescending = true)]
[Index(nameof(UserID))]
[Index(nameof(UserID), nameof(ProductID))]
public class Notification
{

    [Key]
    public int ID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }

    public int UserID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int ProductID { get; set; }

    public string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}