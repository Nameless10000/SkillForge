using AutoMapper;
using Products.Grpc;
using MediatR;
using Grpc.Core;
using SkillForge.Api.Models.Products;
using SkillForge.Api.MediatR.Commands;

namespace SkillForge.Api.MediatR.Handlers.Products;

public class ChangeProductPriceRequestHandler(
    ProductsService.ProductsServiceClient client,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : RequestHandlerBase(httpContextAccessor),
                                                IRequestHandler<ChangeProductPriceReq, ChangeProductPriceResp>
{
    public async Task<ChangeProductPriceResp> Handle(ChangeProductPriceReq request, CancellationToken cancellationToken)
    {

        var meta = new Metadata {
            { "Authorization", _authToken }
        };

        var response = await client.ChangeProductPriceAsync(
            mapper.Map<ChangePriceRequest>(request.ChangeProductPrice),
            meta,
            cancellationToken: cancellationToken);

        return mapper.Map<ChangeProductPriceResp>(response);
    }
}
