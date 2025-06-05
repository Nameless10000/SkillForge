using AutoMapper;
using Products.Grpc;
using MediatR;
using SkillForge.Data.Entities;
using SkillForge.Api.MediatR.Models;
using Grpc.Core;

namespace SkillForge.Api.MediatR.Handlers.Products;

public class GetProductRequestHandler(
    ProductsService.ProductsServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : RequestHandlerBase(httpContextAccessor),
                                                IRequestHandler<GetProductReq, Product?>
{
    public async Task<Product?> Handle(GetProductReq request, CancellationToken cancellationToken)
    {
        var meta = new Metadata {
            { "Authorization", _authToken }
        };

        var response = await client.GetProductAsync(
            mapper.Map<GetProductRequest>(request.GetProduct),
            meta,
            cancellationToken: cancellationToken);

        if (response.Product.ID == 0)
            return null;

        return mapper.Map<Product>(response.Product);
    }
}
