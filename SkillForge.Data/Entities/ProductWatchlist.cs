using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(ProductID), nameof(IsNotified))]
[PrimaryKey(nameof(UserID), nameof(ProductID))]
public class ProductWatchlist
{

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }

    public int UserID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int ProductID { get; set; }

    public decimal DesiredPrice { get; set; }

    public bool IsNotified { get; set; }

}