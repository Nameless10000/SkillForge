using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Data.Entities;

public class Order
{
    [Key]
    public int ID { get; set; }

    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public DateTime CreatedAt { get; set; }
}