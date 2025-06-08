using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Products;

public class ChangeProductPrice
{
    [Required]
    public int ProductID { get; set; }

    [Required]
    public decimal NewPrice { get; set; }
}

public class ChangeProductPriceResp
{
    public bool PriceChanged { get; set; }

    public decimal NewPrice { get; set; }

    public decimal OldPrice { get; set; }
}