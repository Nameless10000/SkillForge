using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Chats;

public class AddToChat
{
    [Required]
    public int ProductID { get; set; }
}