using AutoMapper;
using MediatR;
using Notifications.Grpc;
using SkillForge.Api.MediatR.Commands;
using SkillForge.Api.Utils;

namespace SkillForge.Api.MediatR.Handlers.Notifications;

public class SetIsReadRequestHandler(
    NotificationService.NotificationServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
    ) : RequestHandlerBase(httpContextAccessor), IRequestHandler<SetIsReadReq, bool>
{
    public async Task<bool> Handle(SetIsReadReq request, CancellationToken cancellationToken)
    {
        var meta = httpContextAccessor.HttpContext!.ToAccessTokenMetadata();

        var response = await client.SetIsReadAsync(
            mapper.Map<SetIsReadRequest>(request.SetIsRead),
            meta,
            cancellationToken: cancellationToken
        );

        return response.IsReadSet;
    }
}
