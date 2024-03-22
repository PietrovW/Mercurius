using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Api.Authentication;

internal sealed class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        return Task.FromResult(result);
    }
}
