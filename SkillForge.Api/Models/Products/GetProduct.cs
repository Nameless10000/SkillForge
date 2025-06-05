using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Products;

public class GetProduct {
    [Required]
    public int ProductID { get; set; }
}