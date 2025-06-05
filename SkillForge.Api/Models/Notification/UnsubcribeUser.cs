using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Notifications;

public class UnsubscribeUser
{
    [Required]
    public int ProductID { get; set; }
}