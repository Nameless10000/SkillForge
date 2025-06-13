using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SkillForge.Data;
using SkillForge.Notificator.Services;
using SkillForge.Notificator.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
var envPath = Environment.CurrentDirectory;
var inDev = builder.Environment.IsDevelopment();

builder.Configuration.AddJsonFile(
    inDev
        ? @$"{envPath}/../SkillForge.Data/appsettings.Jwt.json"
        : "/app/appsettings.Jwt.json");

builder.WebHost.ConfigureKestrel(opts =>
    opts.ListenAnyIP(7029, lopts => 
        lopts.UseHttps(inDev 
            ? "../common.pfx"
            : "/app/https/common.pfx", "2174583")
            )
);

builder.Services.AddGrpc();
builder.Services.AddSignalR();

builder.Services.AddCors();

var mapper = new MapperConfiguration(x => x.AddProfile<MapperProfile>()).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<AppDbService>();

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

        // 👇 Это важно для SignalR: вытаскивает токен из query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/notificationsHub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(opt =>
{
    opt.AllowAnyHeader()
        .WithOrigins("http://localhost:8000", "http://localhost:8080")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.MapGrpcService<NotificationServiceImpl>();
app.MapHub<NotificationHub>("/notificationsHub");

app.Run();

