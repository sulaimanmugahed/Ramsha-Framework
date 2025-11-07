using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ramsha.Security.Claims;

public static class PrincipalAccessorExtensions
{
    public static IDisposable Change(this IPrincipalAccessor currentPrincipalAccessor, Claim claim)
    {
        return currentPrincipalAccessor.Change(new[] { claim });
    }

    public static IDisposable Change(this IPrincipalAccessor currentPrincipalAccessor, IEnumerable<Claim> claims)
    {
        return currentPrincipalAccessor.Change(new ClaimsIdentity(claims));
    }

    public static IDisposable Change(this IPrincipalAccessor currentPrincipalAccessor, ClaimsIdentity claimsIdentity)
    {
        return currentPrincipalAccessor.Change(new ClaimsPrincipal(claimsIdentity));
    }
}