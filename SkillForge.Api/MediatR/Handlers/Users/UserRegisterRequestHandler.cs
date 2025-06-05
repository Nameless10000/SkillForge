using MediatR;
using Auth.Grpc;
using SkillForge.Api.Models.Users;
using SkillForge.Api.MediatR.Commands;
using AutoMapper;

namespace SkillForge.Api.MediatR.Handlers;

public class UserRegisterRequestHandler(
    AuthService.AuthServiceClient client,
    IMapper mapper) : IRequestHandler<UserRegisterCommand, AuthedUser>
{

    public async Task<AuthedUser?> Handle(UserRegisterCommand userRegister, CancellationToken cancellationToken)
    {

        var response = await client.RegisterAsync(
            mapper.Map<RegisterRequest>(userRegister.UserRegister),
            cancellationToken: cancellationToken
        );

        return mapper.Map<AuthedUser>(response);
    }

}