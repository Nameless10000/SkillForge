using AutoMapper;
using Products.Grpc;
using SkillForge.Data.Entities;
using Google.Protobuf.WellKnownTypes;

namespace SkillForge.Products.Services;

public class MapperProfile : Profile {
    public MapperProfile()
    {
        CreateMap<AddProductRequest, Product>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTime.Now))
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.ID, opt => opt.Ignore());

        CreateMap<ChangePriceRequest, Product>()
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => DateTime.Now))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.NewPrice));

        CreateMap<User, GrpcSeller>();

        CreateMap<Product, GrpcProduct>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => Timestamp.FromDateTime(x.CreatedAt.ToUniversalTime())))
            .ForMember(x => x.UpdatedAt, opt => opt.Condition(x => x.UpdatedAt != null))
            .ForMember(x => x.UpdatedAt, opt => opt.MapFrom(x => Timestamp.FromDateTime(x.UpdatedAt!.Value.ToUniversalTime())));
    }
}