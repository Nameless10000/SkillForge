using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using SkillForge.Data;
using SkillForge.Data.Entities;

namespace SkillForge.Talks.Services;

public class ChatHub(
    IHttpContextAccessor httpContextAccessor,
    AppDbService appDbService
    ) : Hub
{
    private int _userID => int.Parse(httpContextAccessor
            .HttpContext!
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier)!
        );

    public async Task BackMessage(int sessionID, string message)
    {
        var chatMessage = await appDbService.AddMessageAsync(sessionID, _userID, message);

        System.Console.WriteLine("Back call");

        if (chatMessage == null)
            return;

        var members = await appDbService.GetChatMembersAsync(sessionID);

        await Clients
            .Users(
                members.Select(x => x.ID.ToString())
                )
            .SendAsync("newMessage", chatMessage);
    }
}