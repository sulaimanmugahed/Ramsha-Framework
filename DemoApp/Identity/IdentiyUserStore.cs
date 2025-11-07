using DemoApp;
using DemoApp.Identity;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;


public class RamshaUserStore(IIdentityUserRepository identityUserRepository, IIdentityRoleRepository identityRoleRepository)
: RamshaUserStore<Guid>(identityUserRepository, identityRoleRepository)
{

}

public class RamshaUserStore<TId>(
    IIdentityUserRepository<TId> identityUserRepository,
    IIdentityRoleRepository<TId> identityRoleRepository)
: RamshaUserStore<RamshaIdentityUser<TId>, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>, RamshaIdentityUserClaim<TId>, RamshaIdentityUserLogin<TId>, RamshaIdentityUserToken<TId>, RamshaIdentityRole<TId>>(identityUserRepository, identityRoleRepository)
where TId : IEquatable<TId>
{

}


public class RamshaUserStore<TUser, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TRole>(
    IIdentityUserRepository<TUser, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TRole> userRepository,
    IIdentityRoleRepository<TRole, TId, TUserRole, TRoleClaim> roleRepository) :
    IUserStore<TUser>,
    IUserLoginStore<TUser>,
    IUserRoleStore<TUser>,
    IUserClaimStore<TUser>,
    IUserPasswordStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IUserEmailStore<TUser>,
    IUserLockoutStore<TUser>,
    IUserPhoneNumberStore<TUser>,
    IQueryableUserStore<TUser>,
    IUserTwoFactorStore<TUser>,
    IUserAuthenticationTokenStore<TUser>
     where TId : IEquatable<TId>
     where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
where TUserClaim : RamshaIdentityUserClaim<TId>
where TUserRole : RamshaIdentityUserRole<TId>
where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
{
    public IQueryable<TUser> Users => throw new NotImplementedException();

    private void ThrowIfUserNull(TUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
    }
    public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfUserNull(user);

        user.AddClaims(claims);
    }

    public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfUserNull(user);

        user.AddLogin(login);
    }

    public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfUserNull(user);

        var role = await roleRepository.FindAsync(x => x.Name == roleName);
        user.AddRole(role.Id);
    }

    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfUserNull(user);


        try
        {
            var result = await userRepository.CreateAsync(user);

            if (!result.Equals(default(TId)))
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError[]
            {
                    new IdentityError{ Description = ex.Message }
            });
        }
    }

    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfUserNull(user);


        try
        {
            var result = await userRepository.DeleteAsync(user.Id);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError[]
            {
                    new IdentityError{ Description = ex.Message }
            });
        }
    }

    public void Dispose()
    {

    }

    public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(normalizedEmail))
            throw new ArgumentNullException(nameof(normalizedEmail));

        try
        {
            var result = await userRepository.FindAsync(x => x.NormalizedEmail == normalizedEmail);

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        try
        {
            var key = default(TId);

            var converter = TypeDescriptor.GetConverter(typeof(TId));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                key = (TId)converter.ConvertFromInvariantString(userId);
            }
            else
            {
                key = (TId)Convert.ChangeType(userId, typeof(TId));
            }

            var result = await userRepository.FindAsync(key);

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var result = await userRepository.FindAsync(x => x.Logins.Any(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey));

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(normalizedUserName))
            throw new ArgumentNullException(nameof(normalizedUserName));

        try
        {
            var result = await userRepository.FindAsync(x => x.NormalizedUserName == normalizedUserName);

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.AccessFailedCount);
    }

    public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return user.Claims.Select(x => x.ToClaim()).ToList();
    }

    public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.LockoutEnabled);
    }

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.LockoutEnd);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return user.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName)).ToList();
    }

    public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.NormalizedEmail);
    }

    public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.PasswordHash);
    }

    public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.PhoneNumber);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// //
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>

    public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return [];

    }

    public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.SecurityStamp);
    }

    public Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        return Task.FromResult(string.Empty);
    }

    public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return Task.FromResult(user.UserName);
    }

    public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (claim == null)
            throw new ArgumentNullException(nameof(claim));

        return [];
    }

    public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentNullException(nameof(roleName));


        var role = await roleRepository.FindAsync(x => x.Name == roleName);

        var userIds = role?.Users.Select(x => x.UserId) ?? [];
        return await userRepository.GetListAsync(x => userIds.Contains(x.Id));

    }

    public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.PasswordHash != null);
    }

    public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.AccessFailedCount++;
        return Task.FromResult(user.AccessFailedCount);
    }

    public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentNullException(nameof(roleName));

        var role = await roleRepository.FindAsync(x => x.Name == roleName);

        return user.IsInRole(role.Id);

    }

    public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (claims == null)
            throw new ArgumentNullException(nameof(claims));

        user.RemoveClaims(claims);
    }

    public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentNullException(nameof(roleName));

        var role = await roleRepository.FindAsync(x => x.Name == roleName);


        user.RemoveRole(role.Id);
    }

    public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(loginProvider))
            throw new ArgumentNullException(nameof(loginProvider));

        if (string.IsNullOrEmpty(providerKey))
            throw new ArgumentNullException(nameof(providerKey));

        user.RemoveLogin(loginProvider, providerKey);
    }

    public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (claim == null)
            throw new ArgumentNullException(nameof(claim));

        if (newClaim == null)
            throw new ArgumentNullException(nameof(newClaim));

        user.ReplaceClaim(claim, newClaim);
    }

    public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.AccessFailedCount = 0;

        return Task.FromResult(0);
    }

    public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.Email = email;

        return Task.FromResult(0);
    }

    public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.EmailConfirmed = confirmed;

        return Task.FromResult(0);
    }

    public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.LockoutEnabled = enabled;

        return Task.FromResult(0);
    }

    public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.LockoutEnd = lockoutEnd;

        return Task.FromResult(0);
    }

    public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.NormalizedEmail = normalizedEmail;

        return Task.FromResult(0);
    }

    public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.NormalizedUserName = normalizedName;

        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.PasswordHash = passwordHash;

        return Task.FromResult(0);
    }

    public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.PhoneNumber = phoneNumber;

        return Task.FromResult(0);
    }

    public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.PhoneNumberConfirmed = confirmed;

        return Task.FromResult(0);
    }

    public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.SecurityStamp = stamp;

        return Task.FromResult(0);
    }

    public Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.TwoFactorEnabled = enabled;

        return Task.FromResult(0);
    }

    public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.UserName = userName;

        return Task.FromResult(0);
    }

    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return IdentityResult.Success;
    }

}





// public class IdentityUserStore :
//     IUserStore<IdentityUser>,
//     IUserPasswordStore<IdentityUser>,
//     IUserEmailStore<IdentityUser>
// {
//     private readonly IdentityUserRepository _repository;

//     public IdentityUserStore(IdentityUserRepository repository)
//     {
//         _repository = repository;
//     }

//     // ----------------------
//     // IUserStore
//     // ----------------------
//     public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
//     {
//         if (string.IsNullOrEmpty(user.Id))
//             user.Id = Guid.NewGuid().ToString();

//         return _repository.AddAsync(user)
//             .ContinueWith(_ => IdentityResult.Success);
//     }

//     public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(IdentityResult.Success);

//     public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(IdentityResult.Success);

//     public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
//         => _repository.FindByIdAsync(userId);

//     public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
//         => _repository.FindByNameAsync(normalizedUserName);

//     public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.Id);

//     public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.UserName);

//     public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
//     {
//         user.UserName = userName;
//         return Task.CompletedTask;
//     }

//     public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.NormalizedUserName);

//     public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
//     {
//         user.NormalizedUserName = normalizedName;
//         return Task.CompletedTask;
//     }

//     public void Dispose() { }

//     // ----------------------
//     // IUserPasswordStore
//     // ----------------------
//     public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
//     {
//         user.PasswordHash = passwordHash;
//         return Task.CompletedTask;
//     }

//     public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.PasswordHash);

//     public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

//     // ----------------------
//     // IUserEmailStore
//     // ----------------------
//     public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
//     {
//         user.Email = email;
//         return Task.CompletedTask;
//     }

//     public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.Email);

//     public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(true);

//     public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
//         => Task.CompletedTask;

//     public Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
//         => _repository.FindByEmailAsync(normalizedEmail);

//     public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
//         => Task.FromResult(user.NormalizedEmail);

//     public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
//     {
//         user.NormalizedEmail = normalizedEmail;
//         return Task.CompletedTask;
//     }
// }
