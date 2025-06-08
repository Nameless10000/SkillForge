using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notifications.Grpc;
using SkillForge.Data;
using SkillForge.Notificator.SignalRHubs;

namespace SkillForge.Notificator.Services;

[Authorize]
public class NotificationServiceImpl(
    AppDbService appDbService,
    IHubContext<NotificationHub> hubContext,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
) : NotificationService.NotificationServiceBase
{
    private int _userID => int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public override async Task<GetUserNotificationsResponse> GetUserNotifications(GetUserNotificationsRequest request, ServerCallContext context)
    {
        var userNotifications = await appDbService.GetUserNotificationsAsync(request.UserID);

        var response = new GetUserNotificationsResponse
        {
            NotRead = userNotifications.Count(x => !x.IsRead)
        };

        response.Notofications.AddRange(
            userNotifications.Select(mapper.Map<GrpcNotification>)
            );

        return response;
    }

    public override async Task<ProductPriceChangedResponse> NotifyProductPriceChanged(ProductPriceChangedRequest request, ServerCallContext context)
    {
        var interestedUsers = await appDbService.GetInterestedUsersAsync(request.ProductID);

        var watchlist = interestedUsers
            .Where(x => x.DesiredPrice >= (decimal)request.NewPrice)
            .ToList();

        await hubContext.Clients
            .Users(
                watchlist.Select(x => x.UserID.ToString())
                )
            .SendAsync("ProductPriceChanged", request);

        _ = await appDbService.SetWatchlistNotified(watchlist);

        return new()
        {
            NotifiedUsersCount = watchlist.Count
        };
    }

    public override async Task<SetIsReadResponse> SetIsRead(SetIsReadRequest request, ServerCallContext context)
    {
        var res = await appDbService.SetIsReadToNotificationAsync(request.NotificationID);

        return new()
        {
            IsReadSet = res
        };
    }

    public override async Task<SubscribeToProductResponse> SubscribeToProduct(SubscribeToProductRequest request, ServerCallContext context)
    {
        var res = await appDbService.SubscribeToProductAsync(_userID, request.ProductID, (decimal)request.DesiredPrice);

        return new()
        {
            Subscribed = res
        };
    }

    public override async Task<UnsubscribefromProductResponse> UnsubscribefromProduct(UnsubscribefromProductRequest request, ServerCallContext context)
    {
        var res = await appDbService.UnsubscribeFromProductAsync(_userID, request.ProductID);

        return new()
        {
            Unsubscribed = res
        };
    }

}