using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Authorization.Decision;

internal sealed class DecisionRequirementHandler : AuthorizationHandler<DecisionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsMonitor<JwtBearerOptions> _options;

    public DecisionRequirementHandler(IHttpContextAccessor httpContextAccessor, IOptionsMonitor<JwtBearerOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DecisionRequirement requirement)
    {
        var options = _options.Get(JwtBearerDefaults.AuthenticationScheme);
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null && httpContext.User.Identity != null && !httpContext.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

       if(httpContext!.User.Claims.Any(w => w.Type == "role" && w.Value == requirement.Role))
        {
            context.Succeed(requirement);
            return;
        }
        context.Fail();
    }
}
