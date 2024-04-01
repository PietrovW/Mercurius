using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace Api.Authentication;

internal sealed class RolesClaimsTransformation : IClaimsTransformation
{
    private readonly string _roleClaimType;
   
    public RolesClaimsTransformation(string roleClaimType)
    {
        _roleClaimType = roleClaimType;
    }
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        return Task.FromResult(result);
        if (result.Identity is not ClaimsIdentity identity)
        {
            return Task.FromResult(result);
        }

        var resourceAccessValue = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(resourceAccessValue))
        {
            return Task.FromResult(result);
        }

        using var resourceAccess = JsonDocument.Parse(resourceAccessValue);
        var clientRoles = resourceAccess
            .RootElement
           //  .GetProperty("realm-role")
            .GetProperty("roles");

        foreach (var role in clientRoles.EnumerateArray())
        {
            var value = role.GetString();
            if (!string.IsNullOrWhiteSpace(value))
            {
                identity.AddClaim(new Claim(_roleClaimType, value));
            }
        }

        return Task.FromResult(result);
    }
}
