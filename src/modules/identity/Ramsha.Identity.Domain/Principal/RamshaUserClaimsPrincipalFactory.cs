using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ramsha.Security.Claims;

namespace Ramsha.Identity.Domain;

public class RamshaUserClaimsPrincipalFactory(
   UserManager<RamshaIdentityUser> userManager,
        IOptions<IdentityOptions> options,
        IPrincipalAccessor principalAccessor,
        IRamshaClaimsPrincipalFactory ramshaClaimsPrincipalFactory
)
: UserClaimsPrincipalFactory<RamshaIdentityUser>(userManager, options)
{
    public async override Task<ClaimsPrincipal> CreateAsync(RamshaIdentityUser user)
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

