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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var envPath = Environment.CurrentDirectory;
builder.Configuration.AddJsonFile(@$"{envPath}/../SkillForge.Auth/appsettings.Jwt.json");
builder.Configuration.AddJsonFile(@$"{envPath}/../SkillForge.Data/appsettings.Urls.json");

//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var mapper = new MapperConfiguration(x => x.AddProfile<MapperProfile>()).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.Configure<ServicesUrls>(
    builder.Configuration.GetSection("ServicesUrls")
    );

builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var authChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Auth);
    return new AuthService.AuthServiceClient(authChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var productsChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Products);
    return new ProductsService.ProductsServiceClient(productsChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var notifiationChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Notifications);
    return new NotificationService.NotificationServiceClient(notifiationChannel);
});
builder.Services.AddTransient(opt =>
{
    var servicesUrlsOpts = opt.GetRequiredService<IOptions<ServicesUrls>>();
    var chatChannel = GrpcChannel.ForAddress(servicesUrlsOpts.Value.Talks);
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
            )
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

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(e => e.MapGraphQL());
app.MapGraphQL("/gql");
//app.MapControllers();

app.Run();
