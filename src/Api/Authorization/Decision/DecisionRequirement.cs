using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Decision;

public sealed class DecisionRequirement : IAuthorizationRequirement
{
    public string Role { get; }

    public DecisionRequirement(string role)
    {
        Role = role;
    }
}
