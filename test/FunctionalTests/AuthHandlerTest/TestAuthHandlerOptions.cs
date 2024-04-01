using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace FunctionalTests.AuthHandlerTest;

public class TestAuthHandlerOptions :  AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}
public sealed class DecisionRequirementOptions : IAuthorizationRequirement
{
    public string Role { get; }

    public DecisionRequirementOptions(string role)
    {
        Role = role;
    }
}


