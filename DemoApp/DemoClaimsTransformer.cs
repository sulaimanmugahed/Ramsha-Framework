using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Security.Claims;

namespace DemoApp;

public class DemoClaimsTransformerBefore : IRamshaClaimsPrincipalTransformer
{
    public Task TransformAsync(RamshaClaimsPrincipalContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        identity?.AddClaim(new System.Security.Claims.Claim("demo", "this is demo claim"));
        return Task.CompletedTask;
    }
}

public class DemoClaimsTransformerAfter : IRamshaClaimsPrincipalTransformer
{
    public Task TransformAsync(RamshaClaimsPrincipalContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        identity?.AddClaim(new System.Security.Claims.Claim("demo", "this is demo claim"));
        return Task.CompletedTask;
    }
}

public class DemoClaimsTransformer : IRamshaClaimsPrincipalTransformer
{
    public Task TransformAsync(RamshaClaimsPrincipalContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        identity?.AddClaim(new System.Security.Claims.Claim("demo", "this is demo claim"));
        return Task.CompletedTask;
    }
}
