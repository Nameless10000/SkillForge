using Microsoft.EntityFrameworkCore;
using SkillForge.Auth.Services;
using SkillForge.Data;

var builder = WebApplication.CreateBuilder(args);

var inDev = builder.Environment.IsDevelopment();
builder.WebHost.ConfigureKestrel(opts =>
    opts.ListenAnyIP(7222, lopts => 
        lopts.UseHttps(inDev 
            ? "../common.pfx"
            : "/app/https/common.pfx", "2174583")
            )
);

var envPath = Environment.CurrentDirectory;
builder.Configuration.AddJsonFile(
    inDev
        ? @$"{envPath}/../SkillForge.Data/appsettings.Jwt.json"
        : "/app/appsettings.Jwt.json");
// Add services to the container.

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
