using AutoMapper;
using Chat.Grpc;
using Google.Protobuf.WellKnownTypes;
using SkillForge.Data.Entities;

namespace SkillForge.Talks.Services;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ChatMessage, GrpcChatMessage>()
            .ForMember(x => x.SentAt, opt => opt.MapFrom(x => Timestamp.FromDateTime(x.SentAt.ToUniversalTime())));
    }
}