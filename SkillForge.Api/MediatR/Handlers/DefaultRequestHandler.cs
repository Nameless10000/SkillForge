using System.Security.Claims;
using MediatR;

namespace SkillForge.Api.MediatR.Handlers;

public abstract class RequestHandlerBase(
    IHttpContextAccessor contextAccessor
)
{

    protected string _authToken => contextAccessor.HttpContext!.Request.Headers.Authorization!;

    protected string _userID => contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

}