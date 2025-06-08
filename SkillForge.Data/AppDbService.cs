using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SkillForge.Data.Entities;

namespace SkillForge.Data;

/// <summary>
/// This service must be used in bound with AppDbContext db context registered in DI.
/// </summary>
/// <param name="appDbContext">Application database context</param>
public class AppDbService(AppDbContext appDbContext)
{
    // methods to access & manipulate data
    #region Chats

    public async Task<List<User>> GetChatMembersAsync(int sessionID)
    {
        return await appDbContext.ChatSessions
            .Include(x => x.Buyer)
            .Where(x => x.ID == sessionID)
            .Select(x => x.Buyer)
            .ToListAsync();
    }

    public async Task<(bool Added, bool AlreadyIn)> AddToChatAsync(
        int sellerID,
        int productID,
        int userID
    )
    {
        var session = await appDbContext.ChatSessions
            .FirstOrDefaultAsync(x => x.BuyerID == userID
                && x.SellerID == sellerID
                && x.ProductID == productID);

        if (session != null)
            return (false, true);

        session = new ChatSession
        {
            SellerID = sellerID,
            ProductID = productID,
            BuyerID = userID,
            StartedAt = DateTime.Now.ToUniversalTime()
        };

        await appDbContext.ChatSessions.AddAsync(session);
        await appDbContext.SaveChangesAsync();

        return (true, false);
    }

    public async Task<List<ChatMessage>> GetSessionMessagesAsync(int sessionID)
    {
        return await appDbContext.ChatMessages
            .Where(x => x.SessionID == sessionID)
            .ToListAsync();
    }

    public async Task<bool> QuitChatAsync(int sessionID)
    {
        var session = await appDbContext.ChatSessions
            .FirstOrDefaultAsync(x => x.ID == sessionID);

        if (session == null)
            return false;

        appDbContext.ChatSessions.Remove(session);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<ChatMessage?> AddMessageAsync(
        int sessionID,
        int senderID,
        string message
    )
    {
        if (string.IsNullOrWhiteSpace(message))
            return null;

        var chatMessage = new ChatMessage
        {
            SessionID = sessionID,
            SenderID = senderID,
            Message = message,
            SentAt = DateTime.Now.ToUniversalTime()
        };

        await appDbContext.ChatMessages.AddAsync(chatMessage);
        await appDbContext.SaveChangesAsync();

        return chatMessage;
    }

    #endregion

    #region User

    private SHA256 hasher => SHA256.Create();

    private string HashPassword(string password)
    {
        var passBytes = Encoding.UTF8.GetBytes(password);
        var passHashedBytes = hasher.ComputeHash(passBytes);
        return Encoding.UTF8.GetString(passHashedBytes);
    }

    public async Task<User?> LoginUserAsync(string username, string password)
    {
        var passwordHash = HashPassword(password);
        return await appDbContext.Users
            .FirstOrDefaultAsync(x => x.Username == username && x.PasswordHash == passwordHash);
    }

    public async Task<User?> RegisterUserAsync(string username, string password, string email)
    {
        System.Console.WriteLine(username, password, email);
        var existUser = await appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (existUser != null)
            return null;

        var newUser = new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Email = email,
            CreatedAt = DateTime.UtcNow
        };

        await appDbContext.Users.AddAsync(newUser);
        await appDbContext.SaveChangesAsync();

        return newUser;
    }

    #endregion

    #region Products

    public async Task<Product> AddNewProductAsync(Product product)
    {
        await appDbContext.Products.AddAsync(product);
        await appDbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> GetProductAsync(int id)
        => await appDbContext.Products.Include(x => x.Seller).FirstOrDefaultAsync(x => x.ID == id);

    public async Task<(List<Product> data, int total)> GetProductsBySellerAsync(int sellerID, int offset, int count)
    {

        var total = await appDbContext.Products
            .CountAsync(x => x.SellerID == sellerID);

        return (await appDbContext.Products
            .Include(x => x.Seller)
            .Where(x => x.SellerID == sellerID)
            .Skip(offset)
            .Take(count)
            .ToListAsync(), total);
    }

    public async Task<bool> DeleteProductAsync(int productID, int userID)
    {
        var product = await GetProductAsync(productID);

        if (product == null || product.SellerID != userID)
            return false;

        appDbContext.Products.Remove(product);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Product?> ChangeProductPriceAsync(Product product, decimal oldPrice, int userID)
    {
        if (product.SellerID != userID)
            return null;

        appDbContext.Products.Update(product);

        var newPriceChangeHistory = new ProductPriceHistory
        {
            ChangedAt = DateTime.Now,
            NewPrice = product.Price,
            OldPrice = oldPrice,
            ProductID = product.ID
        };

        await appDbContext.ProductsPriceHistory.AddAsync(newPriceChangeHistory);
        await appDbContext.SaveChangesAsync();

        return product;
    }

    #endregion

    #region Notifications

    public async Task<List<Notification>> GetUserNotificationsAsync(int userID)
    {
        return await appDbContext.Notifications
            .Where(x => x.UserID == userID)
            .ToListAsync();
    }

    public async Task<bool> SetWatchlistNotified(List<ProductWatchlist> productWatchlists)
    {
        foreach (var item in productWatchlists)
            item.IsNotified = true;

        appDbContext.UpdateRange(productWatchlists);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<ProductWatchlist>> GetInterestedUsersAsync(int productID)
    {
        return await appDbContext.ProductsWatchlist
            .Include(x => x.Product)
            .Where(x => x.ProductID == productID && !x.IsNotified)
            .ToListAsync();
    }

    public async Task<bool> SetIsReadToNotificationAsync(int notificationID)
    {
        var notification = await appDbContext.Notifications
            .FirstOrDefaultAsync(x => x.ID == notificationID);

        if (notification == null)
            return false;

        notification.IsRead = true;

        appDbContext.Notifications.Update(notification);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnsubscribeFromProductAsync(int userID, int productID)
    {
        var existingSub = await appDbContext.ProductsWatchlist
            .FirstOrDefaultAsync(x => x.UserID == userID
                && x.ProductID == productID
                && !x.IsNotified);

        if (existingSub == null)
            return false;

        appDbContext.ProductsWatchlist.Remove(existingSub);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SubscribeToProductAsync(
        int userID,
        int productID,
        decimal desiredPrice
        )
    {
        var existingSub = await appDbContext.ProductsWatchlist
            .FirstOrDefaultAsync(x => x.UserID == userID
                && x.ProductID == productID
                && !x.IsNotified);

        if (existingSub != null && existingSub.DesiredPrice == desiredPrice)
            return false;
        else if (existingSub != null)
        {
            existingSub.DesiredPrice = desiredPrice;

            appDbContext.ProductsWatchlist.Update(existingSub);
            await appDbContext.SaveChangesAsync();

            return true;
        }

        var newProductWatchlistItem = new ProductWatchlist
        {
            UserID = userID,
            ProductID = productID,
            DesiredPrice = desiredPrice,
            IsNotified = false
        };

        await appDbContext.ProductsWatchlist.AddAsync(newProductWatchlistItem);
        await appDbContext.SaveChangesAsync();

        return true;
    }

    #endregion
}