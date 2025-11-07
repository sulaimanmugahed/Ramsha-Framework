using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DemoApp.Identity;

public class RamshaRoleStore(IIdentityRoleRepository roleRepository)
: RamshaRoleStore<Guid>(roleRepository)
{

}

public class RamshaRoleStore<TId>(IIdentityRoleRepository<TId> roleRepository)
: RamshaRoleStore<RamshaIdentityRole<TId>, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>>
(roleRepository)
where TId : IEquatable<TId>
{

}

public class RamshaRoleStore<TRole, TId, TUserRole, TRoleClaim>(
IIdentityRoleRepository<TRole, TId, TUserRole, TRoleClaim> roleRepository
) :
IRoleStore<TRole>,
IRoleClaimStore<TRole>
      where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
      where TId : IEquatable<TId>
      where TUserRole : RamshaIdentityUserRole<TId>
      where TRoleClaim : RamshaIdentityRoleClaim<TId>
{
    public void Dispose()
    {
    }

    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));
        try
        {
            var result = await roleRepository.CreateAsync(role);

            return result is not null ? IdentityResult.Success : IdentityResult.Failed();
        }
        catch (Exception ex)
        {

            return IdentityResult.Failed(new IdentityError[]
            {
                    new IdentityError{ Description = ex.Message }
            });
        }
    }

    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
            throw new ArgumentNullException(nameof(role));

        try
        {
            await roleRepository.DeleteAsync(role);

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError[]
            {
                    new IdentityError{ Description = ex.Message }
            });
        }
    }

    public virtual TId ConvertIdFromString(string id)
    {
        if (id == null)
            return default;

        return (TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFromInvariantString(id);
    }

    public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(roleId))
            throw new ArgumentNullException(nameof(roleId));

        try
        {
            var result = await roleRepository.FindAsync(ConvertIdFromString(roleId));

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<TRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(normalizedRoleName))
            throw new ArgumentNullException(nameof(normalizedRoleName));

        try
        {
            var result = await roleRepository.FindAsync(x => x.Name == normalizedRoleName);

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return role.Name;
    }

    public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        if (role.Id.Equals(default))
            return null;

        return role.Id.ToString();
    }

    public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return role.Name;
    }
    public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.Name = normalizedName;
    }

    public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.Name = roleName;
    }

    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return IdentityResult.Success;
    }

    public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return role.Claims.Select(x => x.ToClaim()).ToList();
    }

    public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.AddClaim(claim);
    }

    public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.RemoveClaim(claim);
    }
}