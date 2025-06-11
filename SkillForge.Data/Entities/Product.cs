using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(SellerID))]
[Index(nameof(Price))]
[Index(nameof(Price), AllDescending = true)]
public class Product
{

    [Key]
    public int ID { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    [ForeignKey(nameof(SellerID))]
    public User Seller { get; set; }

    public int SellerID { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int CategoryID { get; set; }

    [ForeignKey(nameof(CategoryID))]
    public Category Category { get; set; }

    public string? Base64Photo { get; set; }
}