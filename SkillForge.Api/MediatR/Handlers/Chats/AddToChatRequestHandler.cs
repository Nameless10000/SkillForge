using AutoMapper;
using Chat.Grpc;
using MediatR;
using SkillForge.Api.MediatR.Commands;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Chats;
using SkillForge.Api.Utils;

namespace SkillForge.Api.MediatR.Handlers.Chats;

public class AddToChatRequestHandler(
    IMapper mapper,
    ChatService.ChatServiceClient chatServiceClient,
    IHttpContextAccessor httpContextAccessor
) : RequestHandlerBase(httpContextAccessor), IRequestHandler<AddToChatReq, bool>
{
    public async Task<bool> Handle(AddToChatReq request, CancellationToken cancellationToken)
    {
        var meta = httpContextAccessor.HttpContext!.ToAccessTokenMetadata();

        var response = await chatServiceClient.AddToChatAsync(
            mapper.Map<AddToChatRequest>(request.AddToChat),
            meta,
            cancellationToken: cancellationToken
        );

        return response.Added;
    }
}