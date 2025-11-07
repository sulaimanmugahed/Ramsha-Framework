using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Ramsha.Security.Claims;

namespace DemoApp.Identity;

public class RamshaUserClaimsPrincipalFactory(
   UserManager<IdentityUser> userManager,
        IOptions<IdentityOptions> options,
        IPrincipalAccessor principalAccessor,
        IRamshaClaimsPrincipalFactory ramshaClaimsPrincipalFactory
)
: UserClaimsPrincipalFactory<IdentityUser>(userManager, options)
{
    public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
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
