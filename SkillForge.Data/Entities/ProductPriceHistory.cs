using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(ProductID))]
[Index(nameof(ChangedAt))]
public class ProductPriceHistory
{

    [Key]
    public int ID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int ProductID { get; set; }

    public decimal NewPrice { get; set; }

    public decimal OldPrice { get; set; }

    public DateTime ChangedAt { get; set; }

}
