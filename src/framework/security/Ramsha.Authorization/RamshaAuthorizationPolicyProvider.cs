using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Ramsha.Authorization;

public class RamshaAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionDefinitionStore definitionStore)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        var permission = await definitionStore.GetAsync(policyName);

        if (permission is not null)
        {
            policy = new AuthorizationPolicyBuilder(Array.Empty<string>())
              .AddRequirements(new PermissionAuthorizationRequirement(policyName))
              .RequireAuthenticatedUser()
              .Build();
        }

        return policy;
    }
}