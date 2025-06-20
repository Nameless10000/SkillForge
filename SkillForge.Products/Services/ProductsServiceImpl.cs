using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Notifications.Grpc;
using Products.Grpc;
using SkillForge.Data;
using SkillForge.Data.Entities;

namespace SkillForge.Products.Services;

[Authorize]
public class ProductsServiceImpl(
    AppDbService appDbService,
    IMapper mapper,
    NotificationService.NotificationServiceClient notificationClient,
    IHttpContextAccessor httpContextAccessor) : ProductsService.ProductsServiceBase
{
    private HttpContext _httpContext => httpContextAccessor.HttpContext!;
    private int _userID => int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string _token => _httpContext.Request.Headers.Authorization!;
    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        System.Console.WriteLine(request.ProductID);
        System.Console.WriteLine(request);
        var product = await appDbService.GetProductAsync(request.ProductID);

        if (product == null)
            return new()
            {
                Product = new()
                {
                    Seller = new()
                }
            };

        return new()
        {
            Product = mapper.Map<GrpcProduct>(product)
        };
    }

    public override async Task<GetProductsBySellerResponse> GetProductsBySeller(GetProductsBySellerRequest request, ServerCallContext context)
    {
        var data = await appDbService.GetProductsBySellerAsync(request.SellerID, request.Offset, request.Count);
        var response = new GetProductsBySellerResponse { Total = data.total, Count = data.data.Count };
        response.Products.AddRange(data.data.Select(mapper.Map<GrpcProduct>));
        return response;
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        return new()
        {
            IsDeleted = await appDbService.DeleteProductAsync(request.ProductID, _userID)
        };
    }

    public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
    {
        var newProduct = mapper.Map<Product>(request);
        newProduct.SellerID = _userID;
        await appDbService.AddNewProductAsync(newProduct);

        return new()
        {
            IsAdded = true,
            ProductID = newProduct.ID
        };
    }

    public override async Task<ChangePriceResponse> ChangeProductPrice(ChangePriceRequest request, ServerCallContext context)
    {
        var product = await appDbService.GetProductAsync(request.ProductID);
        if (product == null)
            return new()
            {
                PriceChanged = false,
                NewPrice = 0,
                OldPrice = 0
            };
        var oldPrice = product.Price;
        mapper.Map(request, product);

        var changedProduct = await appDbService.ChangeProductPriceAsync(product, oldPrice, _userID);
        if (changedProduct == null)
            return new()
            {
                PriceChanged = false,
                NewPrice = 0,
                OldPrice = 0
            };

        var meta = new Metadata
        {
            { "Authorization", _token }
        };
        var response = await notificationClient.NotifyProductPriceChangedAsync(new ProductPriceChangedRequest
        {
            ProductID = request.ProductID,
            NewPrice = request.NewPrice
        },
        meta);

        System.Console.WriteLine($"{response.NotifiedUsersCount} users were interested in {request.ProductID} product");

        return new()
        {
            PriceChanged = true,
            OldPrice = (float)oldPrice,
            NewPrice = (float)changedProduct.Price,
        };
    }

    public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        var order = new Order{
            UserID = _userID,
            CreatedAt = DateTime.Now,
            OrderItems = [.. request.OrderItems.Select(mapper.Map<OrderItem>)]
        };

        _ = await appDbService.CreateOrderAsync(order);

        return new() {
            Created = true
        };
    }

    public override async Task<GetUserOrdersResponse> GetUserOrders(GetUserOrdersRequest request, ServerCallContext context)
    {
        var orders = await appDbService.GetUserOrdersAsync(request.UserID);

        var response = new GetUserOrdersResponse();

        response.Orders.AddRange(orders.Select(mapper.Map<GrpcOrder>));

        return response;
    }

    public override async Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
    {
        return new() {
            Deleted = await appDbService.DeleteOrderAsync(request.OrderID, _userID)
        };
    }

    public override async Task<GetProductsByCategoryResponse> GetProductsByCategory(GetProductsByCategoryRequest request, ServerCallContext context)
    {
        var products = await appDbService.GetProductsByCategoryAsync(
                request.CategoryID, 
                request.Offset,
                request.Count
            );

        var response = new GetProductsByCategoryResponse() {
            Count = products.total
        };

        response.Products
            .AddRange(
                products.data.Select(mapper.Map<GrpcProduct>)
                );

        return response;
    }

    public override async Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request, ServerCallContext context)
    {
        var categories = await appDbService.GetCategoriesAsync();

        var response = new GetCategoriesResponse();

        response.Categories.AddRange(categories.Select(mapper.Map<GrpcCategory>));

        return response;
    }
}