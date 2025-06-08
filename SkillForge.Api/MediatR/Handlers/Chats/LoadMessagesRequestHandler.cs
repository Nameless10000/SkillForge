using AutoMapper;
using Chat.Grpc;
using MediatR;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Chats;
using SkillForge.Api.Utils;

namespace SkillForge.Api.MediatR.Handlers.Chats;

public class LoadMessagesRequestHandler(
    IMapper mapper,
    ChatService.ChatServiceClient chatServiceClient,
    IHttpContextAccessor httpContextAccessor
) : RequestHandlerBase(httpContextAccessor), IRequestHandler<LoadMessagesReq, LoadMessagesResp>
{
    public async Task<LoadMessagesResp> Handle(LoadMessagesReq request, CancellationToken cancellationToken)
    {
        var meta = httpContextAccessor.HttpContext!.ToAccessTokenMetadata();

        var response = await chatServiceClient.LoadMessagesAsync(
            mapper.Map<LoadMessagesRequest>(request.LoadMessages),
            meta,
            cancellationToken: cancellationToken
        );

        return mapper.Map<LoadMessagesResp>(response);
    }
}