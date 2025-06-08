using AutoMapper;
using Grpc.Core;
using MediatR;
using Notifications.Grpc;
using SkillForge.Api.MediatR.Commands;

namespace SkillForge.Api.MediatR.Handlers.Notifications;

public class SubscribeUserRequestHandler(
    NotificationService.NotificationServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
    ) : RequestHandlerBase(httpContextAccessor), IRequestHandler<SubscribeUserReq, bool>
{
    public async Task<bool> Handle(SubscribeUserReq request, CancellationToken cancellationToken)
    {
        var meta = new Metadata{
            {"Authorization", _authToken}
        };

        var result = await client.SubscribeToProductAsync(
            mapper.Map<SubscribeToProductRequest>(request.SubcribeUser),
            meta,
            cancellationToken: cancellationToken
        );

        return result.Subscribed;
    }
}