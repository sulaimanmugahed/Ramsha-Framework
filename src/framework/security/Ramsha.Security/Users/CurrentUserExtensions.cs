using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Security.Users;

public static class CurrentUserExtensions
{
    public static string? FindClaimValue(this ICurrentUser currentUser, string claimType)
    {
        return currentUser.FindClaim(claimType)?.Value;
    }

    public static string[] GetClaimsValues(this ICurrentUser currentUser, string claimType)
    {
        return [.. currentUser.GetClaims(claimType).Select(x => x.Value)];
    }

}
