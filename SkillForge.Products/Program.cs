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
builder.Configuration.AddJsonFile(@$"{envPath}/../SkillForge.Auth/appsettings.Jwt.json");
builder.Configuration.AddJsonFile(@$"{envPath}/../SkillForge.Data/appsettings.Urls.json");
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var mapper = new MapperConfiguration(x => x.AddProfile<MapperProfile>()).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.Configure<ServicesUrls>(
    builder.Configuration.GetSection("ServicesUrls")
);

builder.Services.AddTransient(opt =>
{
    var notificationUrl = opt
        .GetRequiredService<IOptions<ServicesUrls>>()
        .Value
        .Notifications;

    var notificationChannel = GrpcChannel.ForAddress(notificationUrl);
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
            )
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

