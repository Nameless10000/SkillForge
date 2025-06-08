using AutoMapper;
using Chat.Grpc;
using MediatR;
using SkillForge.Api.MediatR.Commands;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Chats;
using SkillForge.Api.Utils;

namespace SkillForge.Api.MediatR.Handlers.Chats;

public class QuitChatRequestHandler(
    IMapper mapper,
    ChatService.ChatServiceClient chatServiceClient,
    IHttpContextAccessor httpContextAccessor
) : RequestHandlerBase(httpContextAccessor), IRequestHandler<QuitChatReq, bool>
{
    public async Task<bool> Handle(QuitChatReq request, CancellationToken cancellationToken)
    {
        var meta = httpContextAccessor.HttpContext!.ToAccessTokenMetadata();

        var response = await chatServiceClient.QuitChatAsync(
            mapper.Map<QuitChatRequest>(request.QuitChat),
            meta,
            cancellationToken: cancellationToken
        );

        return response.Quited;
    }
}