using Grpc.Core;

namespace SkillForge.Api.Utils;

public static class Helpers
{

    #region

    public static Metadata ToAccessTokenMetadata(this HttpContext httpContext)
    {
        return new Metadata{
            {"Authorization", httpContext.Request.Headers.Authorization! }
        };
    }

    #endregion
}