using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SkillForge.Data.Entities;

namespace SkillForge.Data;

public class AppDbContext() : DbContext()
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ProductPriceHistory> ProductsPriceHistory { get; set; }
    public DbSet<ProductWatchlist> ProductsWatchlist { get; set; }
    public DbSet<ProductReview> ProductsReviews { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=2174583;Host=localhost;Port=5432;Database=WebShop;Pooling=true;",
            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());

        // Устанавливаем глобальную настройку для DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
