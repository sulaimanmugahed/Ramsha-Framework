using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Ramsha.Core;

namespace Ramsha.Identity.Domain;


public class RamshaIdentityRoleManager<TRole>(
        IRoleStore<TRole> store,
        IEnumerable<IRoleValidator<TRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RamshaIdentityRoleManager<TRole>> logger
) : RamshaIdentityRoleManager<TRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>>(
              store,
              roleValidators,
              keyNormalizer,
              errors,
              logger
)
where TRole : RamshaIdentityRole
{

}


public class RamshaIdentityRoleManager<TRole, TId, TUserRole, TRoleClaim> : RoleManager<TRole>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
   where TId : IEquatable<TId>
    where TUserRole : RamshaIdentityUserRole<TId>
    where TRoleClaim : RamshaIdentityRoleClaim<TId>
{

    public RamshaIdentityRoleManager(
        IRoleStore<TRole> store,
        IEnumerable<IRoleValidator<TRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RamshaIdentityRoleManager<TRole, TId, TUserRole, TRoleClaim>> logger)
        : base(
              store,
              roleValidators,
              keyNormalizer,
              errors,
              logger)
    {

    }

    public override async Task<IdentityResult> UpdateAsync(TRole role)
    {
        var result = await base.UpdateAsync(role);
        if (result.Succeeded)
        {
            // will raise event later
        }

        return result;
    }

    public override async Task<IdentityResult> CreateAsync(TRole role)
    {
        var result = await base.CreateAsync(role);
        if (result.Succeeded)
        {
            // will raise event later
        }

        return result;
    }


    public async override Task<IdentityResult> DeleteAsync(TRole role)
    {
        var result = await base.DeleteAsync(role);
        if (result.Succeeded)
        {
            // will raise event later
        }

        return result;
    }
}
