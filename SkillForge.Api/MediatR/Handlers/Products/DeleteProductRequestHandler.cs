using AutoMapper;
using Products.Grpc;
using MediatR;
using Grpc.Core;
using SkillForge.Api.Models.Products;
using SkillForge.Api.MediatR.Commands;

namespace SkillForge.Api.MediatR.Handlers.Products;

public class DeleteProductRequestHandler(
    ProductsService.ProductsServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : RequestHandlerBase(httpContextAccessor),
                                                IRequestHandler<DeleteProductReq, DeleteProductResp>
{
    public async Task<DeleteProductResp> Handle(DeleteProductReq request, CancellationToken cancellationToken)
    {
        var meta = new Metadata {
            { "Authorization", _authToken }
        };

        var response = await client.DeleteProductAsync(
            mapper.Map<DeleteProductRequest>(request.DeleteProduct),
            meta,
            cancellationToken: cancellationToken);

        return mapper.Map<DeleteProductResp>(response);
    }
}
