using System.Net.Security;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notifications.Grpc;
using SkillForge.Data;
using SkillForge.Data.Infrastructure;
using SkillForge.Products.Services;

var builder = WebApplication.CreateBuilder(args);

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
// Add services to the container.


builder.WebHost.ConfigureKestrel(opts =>
    opts.ListenAnyIP(7211, lopts => 
        lopts.UseHttps(inDev 
            ? "../common.pfx"
            : "/app/https/common.pfx", "2174583")
            )
);

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
    var notificationUrl = opt
        .GetRequiredService<IOptions<ServicesUrls>>()
        .Value
        .Notifications;

    var notificationChannel = GrpcChannel.ForAddress(notificationUrl, new() {
        HttpHandler = handler
    });
    return new NotificationService.NotificationServiceClient(notificationChannel);
});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<AppDbService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddGrpc();

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

builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("RequireAdminRole", policy =>
    //     policy.RequireRole("Admin"));

    // options.AddPolicy("RequireUserRole", policy =>
    //     policy.RequireRole("User"));

    options.AddPolicy("RequireAuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProductsServiceImpl>();

app.Run();

