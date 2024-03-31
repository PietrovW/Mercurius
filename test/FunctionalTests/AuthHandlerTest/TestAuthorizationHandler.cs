using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FunctionalTests.AuthHandlerTest;

public sealed class TestAuthorizationHandler : AuthorizationHandler<DecisionRequirementOptions>
{
    public const string UserId = "UserId";

    public const string AuthenticationScheme = "Test";
    private readonly string _defaultUserId;

    //public TestAuthHandler(
    //    )//
    //{
    //   // _defaultUserId = options.CurrentValue.DefaultUserId;
    //}

    public TestAuthorizationHandler(string defaultUserId)
    {
        _defaultUserId = defaultUserId;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DecisionRequirementOptions requirement) => await Task.Run
               (() =>
               {
                   var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

                   claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));

                   claims.Add(new Claim(ClaimTypes.Role, "rola_add"));

                   var identity = new ClaimsIdentity(claims, AuthenticationScheme);
                   var principal = new ClaimsPrincipal(identity);
                   var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

                   var result = AuthenticateResult.Success(ticket);

                   context.Succeed((IAuthorizationRequirement)result);



               });
}
