using System.Security.Claims;
using HotChocolate.Caching;
using HotChocolate.Authorization;
using MediatR;
using SkillForge.Api.MediatR.Models;
using SkillForge.Api.Models.Users;
using SkillForge.Api.Models.Products;
using SkillForge.Data.Entities;
using SkillForge.Api.MediatR.Commands;

namespace SkillForge.Api.GqlTypes
{
    public class GqlQuery
    {
        public async Task<AuthedUser> LoginUser(UserLogin userLogin, [Service] IMediator mediator)
        {
            return await mediator.Send(new UserLoginReq(userLogin));
        }

        [Authorize]
        public async Task<Product?> GetProduct(GetProduct getProduct, [Service] IMediator mediator)
        {
            return await mediator.Send(new GetProductReq(getProduct));
        }

        [Authorize]
        public async Task<List<Product>> GetProductsBySeller(GetProductsBySeller getProductsBySeller, [Service] IMediator mediator)
        {
            return await mediator.Send(new GetProductsBySellerReq(getProductsBySeller));
        }
    }
}
