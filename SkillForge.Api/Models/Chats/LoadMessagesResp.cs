using SkillForge.Data.Entities;

namespace SkillForge.Api.Models.Chats;

public class LoadMessagesResp
{
    public List<ChatMessage> Messages { get; set; }

    public int UnreadCount { get; set; }
}