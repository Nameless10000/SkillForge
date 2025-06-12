using Microsoft.EntityFrameworkCore;
using SkillForge.Auth.Services;
using SkillForge.Data;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(opts =>
    opts.ListenAnyIP(7222, lopts => lopts.UseHttps("/app/https/common.pfx", "2174583"))
);

var envPath = Environment.CurrentDirectory;
builder.Configuration.AddJsonFile(@$"{envPath}/appsettings.Jwt.json");
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddGrpc();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<AppDbService>();
builder.Services.AddTransient<JwtGenService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var database = scope.ServiceProvider.GetRequiredService<AppDbContext>().Database;
database.Migrate();

app.UseHttpsRedirection();

app.MapGrpcService<AuthServiceImpl>();

app.UseRouting();
// app.UseEndpoints(endpoints => {
//     endpoints.MapGrpcService<AuthServiceImpl>();
// });

app.Run();
