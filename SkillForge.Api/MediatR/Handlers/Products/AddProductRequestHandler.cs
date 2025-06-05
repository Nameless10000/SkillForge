using AutoMapper;
using Products.Grpc;
using MediatR;
using Grpc.Core;
using SkillForge.Api.Models.Products;
using SkillForge.Api.MediatR.Commands;

namespace SkillForge.Api.MediatR.Handlers.Products;

public class AddProductRequestHandler(
    ProductsService.ProductsServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : RequestHandlerBase(httpContextAccessor), 
                                                IRequestHandler<AddProductReq, AddProductResp>
{
    public async Task<AddProductResp> Handle(AddProductReq request, CancellationToken cancellationToken)
    {

        var meta = new Metadata {
            { "Authorization", _authToken }
        };

        var response = await client.AddProductAsync(
            mapper.Map<AddProductRequest>(request.AddProduct),
            meta,
            cancellationToken: cancellationToken);

        return mapper.Map<AddProductResp>(response);
    }
}
