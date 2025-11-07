using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha.Security.Claims;

public class RamshaClaimsPrincipalContext
{
    public ClaimsPrincipal ClaimsPrincipal { get; set; }

    public IServiceProvider ServiceProvider { get; }

    public RamshaClaimsPrincipalContext(
        ClaimsPrincipal claimsIdentity,
        IServiceProvider serviceProvider)
    {
        ClaimsPrincipal = claimsIdentity;
        ServiceProvider = serviceProvider;
    }

    public virtual T GetRequiredService<T>()
        where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}



