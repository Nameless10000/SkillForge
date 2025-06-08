using Auth.Grpc;
using MediatR;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Users;
using SkillForge.Api.Models.Products;
using SkillForge.Data.Entities;
using SkillForge.Api.Models.Notifications;
using SkillForge.Api.Models.Chats;
// using SkillForge.Api.Models.DTO;
// using SkillForge.Api.Models.Entities;

namespace SkillForge.Api.MediatR.Models
{
    public record class UserLoginReq(UserLogin UserLogin) : IRequest<AuthedUser>;

    #region Products

    public record class GetProductReq(GetProduct GetProduct) : IRequest<Product?>;

    public record class GetProductsBySellerReq(GetProductsBySeller GetProductsBySeller) : IRequest<List<Product>>;

    #endregion

    #region Chats

    public record class LoadMessagesReq(LoadMessages LoadMessages) : IRequest<LoadMessagesResp>;

    #endregion
}

namespace SkillForge.Api.MediatR.Commands
{
    public record class UserRegisterCommand(UserRegister UserRegister) : IRequest<AuthedUser>;

    #region Products

    public record class AddProductReq(AddProduct AddProduct) : IRequest<AddProductResp>;
    public record class DeleteProductReq(DeleteProduct DeleteProduct) : IRequest<DeleteProductResp>;
    public record class ChangeProductPriceReq(ChangeProductPrice ChangeProductPrice) : IRequest<ChangeProductPriceResp>;

    #endregion

    #region Notifications

    public record class SubscribeUserReq(SubscribeUser SubcribeUser) : IRequest<bool>;
    public record class UnsubscribeUserReq(UnsubscribeUser UnsubcribeUser) : IRequest<bool>;
    public record class SetIsReadReq(SetIsRead SetIsRead) : IRequest<bool>;

    #endregion

    #region Chats

    public record class AddToChatReq(AddToChat AddToChat) : IRequest<bool>;

    public record class QuitChatReq(QuitChat QuitChat) : IRequest<bool>;

    #endregion
}
