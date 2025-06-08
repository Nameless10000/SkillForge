using System.Security.Claims;
using AutoMapper;
using Chat.Grpc;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using SkillForge.Data;

namespace SkillForge.Talks.Services;

public class ChatServiceImpl(
    AppDbService appDbService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    IHubContext<ChatHub> hubContext
) : ChatService.ChatServiceBase
{

    private int _userID => int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    public override async Task<AddToChatResponse> AddToChat(AddToChatRequest request, ServerCallContext context)
    {
        var result = await appDbService.AddToChatAsync(
            request.SellerID,
            request.ProductID,
            _userID
            );

        return new AddToChatResponse
        {
            Added = result.Added,
            AlreadyAdded = result.AlreadyIn
        };
    }

    public override async Task<LoadMessagesResponse> LoadMessages(LoadMessagesRequest request, ServerCallContext context)
    {
        var messages = await appDbService.GetSessionMessagesAsync(request.SessionID);
        var response = new LoadMessagesResponse
        {
            UnreadCount = messages.Count(x => !x.IsRead)
        };

        response.Messages.AddRange(messages.Select(mapper.Map<GrpcChatMessage>));

        return response;
    }

    public override async Task<QuitChatResponse> QuitChat(QuitChatRequest request, ServerCallContext context)
    {
        var quited = await appDbService.QuitChatAsync(request.SessionID);

        return new()
        {
            Quited = quited
        };
    }

    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        var chatMessage = await appDbService.AddMessageAsync(
            request.SessionID,
            _userID,
            request.Message
            );

        if (chatMessage == null)
            return new()
            {
                Sent = false
            };

        var members = await appDbService.GetChatMembersAsync(request.SessionID);

        // call SignalR
        await hubContext.Clients
            .Users(
                members.Select(x => x.ID.ToString())
            )
            .SendAsync("newMessage", chatMessage);

        return new SendMessageResponse
        {
            Sent = true
        };
    }
}