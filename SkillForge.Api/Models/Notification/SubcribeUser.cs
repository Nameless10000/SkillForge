using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Notifications;

public class SubscribeUser
{
    [Required]
    public int ProductID { get; set; }

    [Required]
    public decimal DesiredPrice { get; set; }
}