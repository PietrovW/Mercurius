﻿using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Decision;

public class DecisionRequirement : IAuthorizationRequirement
{
    public string Resource { get; }
    public string Scope { get; }

    public DecisionRequirement(string resource, string scope)
    {
        Resource = resource;
        Scope = scope;
    }
}
