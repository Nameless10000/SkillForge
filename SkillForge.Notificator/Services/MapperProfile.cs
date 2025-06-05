using AutoMapper;
using SkillForge.Data.Entities;
using Notifications.Grpc;
using Google.Protobuf.WellKnownTypes;

namespace SkillForge.Notificator.Services;

public class MapperProfile : Profile {
    public MapperProfile()
    {
        CreateMap<Notification, GrpcNotification>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => Timestamp.FromDateTime(x.CreatedAt.ToUniversalTime())));
    }
}