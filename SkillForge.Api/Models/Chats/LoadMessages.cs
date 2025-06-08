using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Chats;

public class LoadMessages
{
    [Required]
    public int SessionID { get; set; }
}