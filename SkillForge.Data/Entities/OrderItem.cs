using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Data.Entities;

public class OrderItem
{
    [Key]
    public int ID { get; set; }

    public int OrderID { get; set; }

    [ForeignKey(nameof(OrderID))]
    public Order Order { get; set; }

    public int ProductID { get; set; }

    [ForeignKey(nameof(ProductID))]
    public Product Product { get; set; }

    public int Quantity { get; set; }
}