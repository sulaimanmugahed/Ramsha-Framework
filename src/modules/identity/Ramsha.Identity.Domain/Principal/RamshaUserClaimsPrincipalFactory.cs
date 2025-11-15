using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ramsha.Security.Claims;

namespace Ramsha.Identity.Domain;

public class RamshaUserClaimsPrincipalFactory<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(
   UserManager<TUser> userManager,
RoleManager<TRole> roleManager,
        IOptions<IdentityOptions> options,
        IPrincipalAccessor principalAccessor,
        IRamshaClaimsPrincipalFactory ramshaClaimsPrincipalFactory
)
: UserClaimsPrincipalFactory<TUser, TRole>(userManager, roleManager, options)
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>, new()
 where TUserClaim : RamshaIdentityUserClaim<TId>, new()
 where TUserRole : RamshaIdentityUserRole<TId>, new()
 where TUserLogin : RamshaIdentityUserLogin<TId>, new()
where TUserToken : RamshaIdentityUserToken<TId>, new()
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>, new()
where TRoleClaim : RamshaIdentityRoleClaim<TId>, new()
{
    public async override Task<ClaimsPrincipal> CreateAsync(TUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = principal.Identities.First();

        if (!string.IsNullOrEmpty(user.Email))
        {
            identity.AddClaim(new Claim(RamshaClaimsTypes.Email, user.Email));
        }

        using (principalAccessor.Change(identity))
        {
            await ramshaClaimsPrincipalFactory.CreateAsync(principal);
        }

        return principal;
    }
}

