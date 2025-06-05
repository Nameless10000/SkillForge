using Auth.Grpc;
using Grpc.Core;
using SkillForge.Data;

namespace SkillForge.Auth.Services;

public class AuthServiceImpl(
    AppDbService appDbService, 
    JwtGenService jwtGen) : AuthService.AuthServiceBase {
    public override async Task<AuthResponse> Register(RegisterRequest request, ServerCallContext context) {
        var user = await appDbService.RegisterUserAsync(request.Username, request.Password, request.Email);

        if (user == null)
            return new AuthResponse {
                AccessToken = ""
            };

        System.Console.WriteLine(user);
        var accessToken = jwtGen.GenerateToken(user);

        return new AuthResponse {
            AccessToken = accessToken
        };
    }

    public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context) {
        var user = await appDbService.LoginUserAsync(request.Username, request.Password);

        if (user == null)
            return new AuthResponse {
                AccessToken = ""
            };

        var accessToken = jwtGen.GenerateToken(user);

        return new AuthResponse {
            AccessToken = accessToken
        };
    }
}