using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Products;

public class AddProduct
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int CategoryID { get; set; }
}

public class AddProductResp
{
    public int ProductID { get; set; }

    public bool IsAdded { get; set; }
}