using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(BuyerID))]
[Index(nameof(SellerID))]
[Index(nameof(ProductID), nameof(SellerID))]
[Index(nameof(ProductID), nameof(SellerID), nameof(BuyerID))]
public class ChatSession
{

    [Key]
    public int ID { get; set; }

    [ForeignKey(nameof(BuyerID))]
    public User Buyer { get; set; }

    public int BuyerID { get; set; }

    [ForeignKey(nameof(SellerID))]
    public User Seller { get; set; }

    public int SellerID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int ProductID { get; set; }

    public DateTime StartedAt { get; set; }

}