using Microsoft.AspNetCore.Identity;

using System.Collections.Concurrent;


public class IdentityUserRepository
{
    private readonly ConcurrentDictionary<string, IdentityUser> _users = new();

    public Task AddAsync(IdentityUser user)
    {
        _users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task<IdentityUser?> FindByIdAsync(string id)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<IdentityUser?> FindByNameAsync(string normalizedUserName)
    {
        var user = _users.Values.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName);
        return Task.FromResult(user);
    }

    public Task<IdentityUser?> FindByEmailAsync(string normalizedEmail)
    {
        var user = _users.Values.FirstOrDefault(u => u.NormalizedEmail == normalizedEmail);
        return Task.FromResult(user);
    }
}


public class IdentityUserStore :
    IUserStore<IdentityUser>,
    IUserPasswordStore<IdentityUser>,
    IUserEmailStore<IdentityUser>
{
    private readonly IdentityUserRepository _repository;

    public IdentityUserStore(IdentityUserRepository repository)
    {
        _repository = repository;
    }

    // ----------------------
    // IUserStore
    // ----------------------
    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(user.Id))
            user.Id = Guid.NewGuid().ToString();

        return _repository.AddAsync(user)
            .ContinueWith(_ => IdentityResult.Success);
    }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(IdentityResult.Success);

    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(IdentityResult.Success);

    public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        => _repository.FindByIdAsync(userId);

    public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        => _repository.FindByNameAsync(normalizedUserName);

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.Id);

    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.UserName);

    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedUserName);

    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public void Dispose() { }

    // ----------------------
    // IUserPasswordStore
    // ----------------------
    public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash);

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

    // ----------------------
    // IUserEmailStore
    // ----------------------
    public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.Email);

    public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(true);

    public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        => _repository.FindByEmailAsync(normalizedEmail);

    public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedEmail);

    public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
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
