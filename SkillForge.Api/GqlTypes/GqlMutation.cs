using Auth.Grpc;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Api.MediatR.Commands;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Chats;
using SkillForge.Api.Models.Notifications;
using SkillForge.Api.Models.Products;
using SkillForge.Api.Models.Users;


// using SkillForge.Api.Models.DTO;
using SkillForge.Api.Services;

namespace SkillForge.Api.GqlTypes
{
    public class GqlMutation
    {
        #region Users

        public async Task<AuthedUser> RegisterUser(UserRegister userRegisterer, [Service] IMediator mediator)
        {
            return await mediator.Send(new UserRegisterCommand(userRegisterer));
        }

        #endregion

        #region Products

        [Authorize]
        public async Task<AddProductResp> AddProduct(AddProduct addProduct, [Service] IMediator mediator)
        {
            return await mediator.Send(new AddProductReq(addProduct));
        }

        [Authorize]
        public async Task<DeleteProductResp> DeleteProduct(DeleteProduct deleteProduct, [Service] IMediator mediator)
        {
            return await mediator.Send(new DeleteProductReq(deleteProduct));
        }

        [Authorize]
        public async Task<ChangeProductPriceResp> ChangeProductPrice(ChangeProductPrice changeProductPrice, [Service] IMediator mediator)
        {
            return await mediator.Send(new ChangeProductPriceReq(changeProductPrice));
        }

        #endregion

        #region Notifications

        [Authorize]
        public async Task<bool> SubscribeUserToProduct(SubscribeUser subscribeUser, [Service] IMediator mediator)
        {
            return await mediator.Send(new SubscribeUserReq(subscribeUser));
        }

        [Authorize]
        public async Task<bool> UnsubscribeUserToProduct(UnsubscribeUser unsubscribeUser, [Service] IMediator mediator)
        {
            return await mediator.Send(new UnsubscribeUserReq(unsubscribeUser));
        }

        [Authorize]
        public async Task<bool> MarkNotificationRead(SetIsRead setIsRead, [Service] IMediator mediator)
        {
            return await mediator.Send(new SetIsReadReq(setIsRead));
        }

        #endregion

        #region Chats

        [Authorize]
        public async Task<bool> AddToChat(AddToChat addToChat, [Service] IMediator mediator)
        {
            return await mediator.Send(new AddToChatReq(addToChat));
        }

        [Authorize]
        public async Task<bool> QuitChat(QuitChat quitChat, [Service] IMediator mediator)
        {
            return await mediator.Send(new QuitChatReq(quitChat));
        }

        #endregion
    }
}
