using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Products;

public class GetProductsBySeller
{
    [Required]
    public int SellerID { get; set; }

    public int Offset { get; set; } = 0;

    public int Count { get; set; } = 10;
}
