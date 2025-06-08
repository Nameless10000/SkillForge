using Auth.Grpc;
using AutoMapper;
using Chat.Grpc;
using Notifications.Grpc;
using Products.Grpc;
using SkillForge.Api.Models.Chats;
using SkillForge.Api.Models.Notifications;
using SkillForge.Api.Models.Products;
using SkillForge.Api.Models.Users;
using SkillForge.Data.Entities;

namespace SkillForge.Api.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AuthResponse, AuthedUser>();

            CreateMap<UserLogin, LoginRequest>();
            CreateMap<UserRegister, RegisterRequest>();

            #region Products

            CreateMap<GetProduct, GetProductRequest>();
            CreateMap<GetProductsBySeller, GetProductsBySellerRequest>();

            CreateMap<GrpcProduct, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime()));
            CreateMap<GrpcSeller, User>();

            CreateMap<AddProductResponse, AddProductResp>();
            CreateMap<AddProduct, AddProductRequest>();

            CreateMap<DeleteProductResponse, DeleteProductResp>();
            CreateMap<DeleteProduct, DeleteProductRequest>();

            CreateMap<ChangePriceResponse, ChangeProductPriceResp>();
            CreateMap<ChangeProductPrice, ChangePriceRequest>();

            #endregion

            #region Notification

            CreateMap<SubscribeUser, SubscribeToProductRequest>();

            CreateMap<UnsubscribeUser, UnsubscribefromProductRequest>();

            CreateMap<SetIsRead, SetIsReadRequest>();

            #endregion

            #region Chats

            CreateMap<LoadMessages, LoadMessagesRequest>();
            CreateMap<LoadMessagesResponse, LoadMessagesResp>();
            CreateMap<GrpcChatMessage, ChatMessage>()
                .ForMember(x => x.SentAt, opt => opt.MapFrom(x => x.SentAt.ToDateTime()));

            #endregion
        }
    }
}
