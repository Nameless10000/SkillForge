using MediatR;
using SkillForge.Api.Models.Users;
using Auth.Grpc;
using AutoMapper;
using SkillForge.Api.MediatR.Models;

namespace SkillForge.Api.MediatR.Handlers;

public class UserLoginRequestHandler(
    AuthService.AuthServiceClient client,
    IMapper mapper) : IRequestHandler<UserLoginReq, AuthedUser>
{

    public async Task<AuthedUser?> Handle(UserLoginReq userLogin, CancellationToken cancellationToken)
    {

        var response = await client.LoginAsync(
            mapper.Map<LoginRequest>(userLogin.UserLogin),
            cancellationToken: cancellationToken
        );

        return mapper.Map<AuthedUser>(response);
    }

}