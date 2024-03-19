using Keycloak.AuthServices.Authorization.Requirements;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authorization;

namespace Api.Extensions;

public static class PoliciesBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequireResourceRoles(
    this AuthorizationPolicyBuilder builder, params string[] roles) =>
    builder
        .RequireClaim(KeycloakConstants.ResourceAccessClaimType)
        .AddRequirements(new ResourceAccessRequirement(default, roles));

    public static AuthorizationPolicyBuilder RequireRealmRoles(
        this AuthorizationPolicyBuilder builder, params string[] roles) =>
        builder
            .RequireClaim(KeycloakConstants.RealmAccessClaimType)
            .AddRequirements(new RealmAccessRequirement(roles));
}

