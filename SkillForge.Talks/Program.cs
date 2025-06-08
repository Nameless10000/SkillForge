using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SkillForge.Data;
using SkillForge.Talks.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var envPath = Environment.CurrentDirectory;
builder.Configuration.AddJsonFile(@$"{envPath}/../SkillForge.Auth/appsettings.Jwt.json");

var mapper = new MapperConfiguration(opt => opt.AddProfile<MapperProfile>()).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddGrpc();
builder.Services.AddSignalR();

builder.Services.AddCors();

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
            )
        };

        // 👇 Это важно для SignalR: вытаскивает токен из query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chatHub"))
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
    opt.WithOrigins("localhost:8000", "localhost:8080")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        );

app.MapHub<ChatHub>("/chatHub");
app.MapGrpcService<ChatServiceImpl>();

app.UseHttpsRedirection();

app.Run();
