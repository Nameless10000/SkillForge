using System.ComponentModel.DataAnnotations;

namespace SkillForge.Api.Models.Chats;

public class QuitChat
{
    [Required]
    public int SessionID { get; set; }
}