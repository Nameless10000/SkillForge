using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Products;

public class DeleteProduct
{
    [Required]
    public int ProductID { get; set; }
}

public class DeleteProductResp {
    public bool IsDeleted { get; set; }
}