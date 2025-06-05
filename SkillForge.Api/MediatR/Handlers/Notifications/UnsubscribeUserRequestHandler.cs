using AutoMapper;
using MediatR;
using Notifications.Grpc;
using SkillForge.Api.MediatR.Commands;
using SkillForge.Api.Utils;

namespace SkillForge.Api.MediatR.Handlers.Notifications;

public class UnsubscribeUserRequestHandler(
    NotificationService.NotificationServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
    ) : RequestHandlerBase(httpContextAccessor), IRequestHandler<UnsubscribeUserReq, bool>
{
    public async Task<bool> Handle(UnsubscribeUserReq request, CancellationToken cancellationToken)
    {
        var meta = httpContextAccessor.HttpContext!.ToAccessTokenMetadata();

        var result = await client.UnsubscribefromProductAsync(
            mapper.Map<UnsubscribefromProductRequest>(request.UnsubcribeUser),
            meta,
            cancellationToken: cancellationToken
        );

        return result.Unsubscribed;
    }
}