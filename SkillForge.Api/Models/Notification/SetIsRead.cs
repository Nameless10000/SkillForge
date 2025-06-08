using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Notifications;

public class SetIsRead
{
    [Required]
    public int NotificationID { get; set; }
}