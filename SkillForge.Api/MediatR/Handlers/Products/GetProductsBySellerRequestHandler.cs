using AutoMapper;
using Products.Grpc;
using MediatR;
using SkillForge.Data.Entities;
using SkillForge.Api.MediatR.Models;
using Grpc.Core;

namespace SkillForge.Api.MediatR.Handlers.Products;

public class GetProductsBySellerRequestHandler(
    ProductsService.ProductsServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : RequestHandlerBase(httpContextAccessor),
                                                IRequestHandler<GetProductsBySellerReq, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsBySellerReq request, CancellationToken cancellationToken)
    {
        var meta = new Metadata {
            { "Authorization", _authToken }
        };

        var response = await client.GetProductsBySellerAsync(
            mapper.Map<GetProductsBySellerRequest>(request.GetProductsBySeller),
            meta,
            cancellationToken: cancellationToken);

        return [.. response.Products.Select(mapper.Map<Product>)];
    }
}
