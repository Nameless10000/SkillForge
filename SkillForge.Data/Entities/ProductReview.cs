using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(ProductID))]
public class ProductReview
{

    [Key]
    public int ID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }

    public int UserID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int ProductID { get; set; }

    public double Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

}