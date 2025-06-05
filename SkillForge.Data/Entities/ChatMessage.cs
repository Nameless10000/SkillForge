using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Entities;

[Index(nameof(SessionID))]
[Index(nameof(SenderID))]
public class ChatMessage
{

    [Key]
    public int ID { get; set; }

    [ForeignKey(nameof(SessionID))]
    public ChatSession Session { get; set; }

    public int SessionID { get; set; }

    [ForeignKey(nameof(SenderID))]
    public User Sender { get; set; }

    public int SenderID { get; set; }

    public string Message { get; set; }

    public DateTime SentAt { get; set; }

}