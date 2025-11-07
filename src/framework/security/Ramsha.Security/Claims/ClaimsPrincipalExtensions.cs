using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ramsha.Security.Claims;

namespace System.Security.Principal;

public static class ClaimsPrincipalExtensions
{
    public static bool HasRole(this ClaimsPrincipal principal, string roleName)
    {
        return principal.FindClaims(RamshaClaimsTypes.Role)
        .Any(x => x.Value == roleName);
    }

    public static bool HasRole(this ClaimsIdentity identity, string roleName)
    {
        return identity.FindClaims(RamshaClaimsTypes.Role)
        .Any(x => x.Value == roleName);
    }
    public static string[] GetRoles(this ClaimsPrincipal principal)
    {
        return principal.FindClaims(RamshaClaimsTypes.Role)
        .Select(x => x.Value)
        .Distinct()
        .ToArray();
    }

    public static string[] GetRoles(this ClaimsIdentity identity)
    {
        return identity.FindClaims(RamshaClaimsTypes.Role)
        .Select(x => x.Value)
        .Distinct()
        .ToArray();
    }

    public static Claim[] FindClaims(this ClaimsPrincipal principal, string claimType)
    {
        return [.. principal.Claims.Where(x => x.Type == claimType)];
    }
    public static Claim[] FindClaims(this ClaimsIdentity identity, string claimType)
    {
        return [.. identity.Claims.Where(x => x.Type == claimType)];
    }

    public static string? FindEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(RamshaClaimsTypes.Email)?.Value;
    }

    public static string? FindEmail(this ClaimsIdentity identity)
    {
        return identity.FindFirst(RamshaClaimsTypes.Email)?.Value;
    }
    public static string? FindUsername(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(RamshaClaimsTypes.Username)?.Value;
    }

    public static string? FindUsername(this ClaimsIdentity identity)
    {
        return identity.FindFirst(RamshaClaimsTypes.Username)?.Value;
    }
    public static Guid? FindUserId(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(RamshaClaimsTypes.UserId)?.Value;
        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid id))
        {
            return id;
        }
        return null;
    }

    public static Guid? FindUserId(this ClaimsIdentity identity)
    {
        var userId = identity.FindFirst(RamshaClaimsTypes.UserId)?.Value;
        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid id))
        {
            return id;
        }
        return null;
    }
}
