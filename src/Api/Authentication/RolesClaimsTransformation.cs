using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Api.Authentication;

internal sealed class RolesClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        return Task.FromResult(result);
    }
}
