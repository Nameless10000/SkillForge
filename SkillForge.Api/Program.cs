using AutoMapper;
using Microsoft.Extensions.Options;
using SkillForge.Api.GqlTypes;
using SkillForge.Api.Services;
using SkillForge.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Grpc.Net.Client;
using Auth.Grpc;
using Products.Grpc;
using Notifications.Grpc;
using SkillForge.Data.Infrastructure;
using Chat.Grpc;
using System.Security.Claims;
using System.Net.Security;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var envPath = Environment.CurrentDirectory;
var inDev = builder.Environment.IsDevelopment();

builder.Configuration.AddJsonFile(
    inDev
        ? @$"{envPath}/../SkillForge.Data/appsettings.Urls.json"
        : "/app/appsettings.Urls.json");
builder.Configuration.AddJsonFile(
    inDev
        ? @$"{envPath}/../SkillForge.Data/appsettings.Jwt.json"
        : "/app/appsettings.Jwt.json");

builder.WebHost.ConfigureKestrel(opts =>
    opts.ListenAnyIP(7090, lopts => 
        lopts.UseHttps(inDev 
            ? "../common.pfx"
            : "/app/https/common.pfx", "2174583")
            )
);

//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddCors();

var mapper = new MapperConfiguration(x => x.AddProfile<MapperProfile>()).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.Configure<ServicesUrls>(
    builder.Configuration.GetSection("ServicesUrls")
    );

var handler = new SocketsHttpHandler
{
    SslOptions = new SslClientAuthenticationOptions
    {
        RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            // тут можно как угодно фильтровать, но для dev — просто true
            return true;
        }
    }
};

builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var authChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Auth, new() {
        HttpHandler = handler
    });
    return new AuthService.AuthServiceClient(authChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var productsChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Products, new() {
        HttpHandler = handler
    });
    return new ProductsService.ProductsServiceClient(productsChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var notifiationChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Notifications, new() {
        HttpHandler = handler
    });
    return new NotificationService.NotificationServiceClient(notifiationChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var chatChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Talks, new() {
        HttpHandler = handler
    });
    return new ChatService.ChatServiceClient(chatChannel);
});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<AppDbService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Program>()
);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetValue<string>("Jwt:SecretKey")!
                    )
                ),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddGraphQLServer()
    .AddQueryType<GqlQuery>()
    .AddMutationType<GqlMutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization();

var app = builder.Build();

app.UseCors(opt => opt
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:8001")
    .AllowCredentials()
    );

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(e => e.MapGraphQL());
app.MapGraphQL("/gql");
//app.MapControllers();

app.Run();
