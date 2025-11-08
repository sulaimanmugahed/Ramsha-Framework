using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ramsha.Security.Claims;

namespace Ramsha.Identity.Domain;

public class RamshaUserClaimsPrincipalFactory<TUser,TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(
   UserManager<TUser> userManager,
        IOptions<IdentityOptions> options,
        IPrincipalAccessor principalAccessor,
        IRamshaClaimsPrincipalFactory ramshaClaimsPrincipalFactory
)
: UserClaimsPrincipalFactory<TUser>(userManager, options)
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
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

