using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Core;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;
using Ramsha.Identity.Domain;
using Ramsha.Common.Application;

namespace Ramsha.Identity.Application;

public class RamshaIdentityUserService<TUser, TDto, TCreateDto, TUpdateDto>(RamshaIdentityUserManager<TUser> userManager, IIdentityUserRepository<TUser, Guid> repository)
: RamshaIdentityUserService<TUser, RamshaIdentityRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>, TDto, TCreateDto, TUpdateDto>(userManager, repository)
where TUser : RamshaIdentityUser, new()
where TCreateDto : CreateRamshaIdentityUserDto, new()
where TUpdateDto : UpdateRamshaIdentityUserDto, new()
where TDto : RamshaIdentityUserDto, new()
{

}


public class RamshaIdentityUserService<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TDto, TCreateDto, TUpdateDto>(RamshaIdentityUserManager<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken> userManager, IIdentityUserRepository<TUser, TId> repository) : RamshaService, IRamshaIdentityUserService<TDto, TCreateDto, TUpdateDto, TId>
where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>, new()
where TUserClaim : RamshaIdentityUserClaim<TId>, new()
where TUserRole : RamshaIdentityUserRole<TId>, new()
where TUserLogin : RamshaIdentityUserLogin<TId>, new()
where TUserToken : RamshaIdentityUserToken<TId>, new()
where TRoleClaim : RamshaIdentityRoleClaim<TId>, new()
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>, new()
where TDto : RamshaIdentityUserDto, new()
where TCreateDto : CreateRamshaIdentityUserDto, new()
where TUpdateDto : UpdateRamshaIdentityUserDto, new()

{

    public virtual async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        var user = MapCreate(createDto);
        var identityResult = await userManager.CreateAsync(user, createDto.Password);
        if (!identityResult.Succeeded)
        {
            return identityResult.MapToRamshaErrors();
        }

        if (createDto.Roles is not null)
        {
            var roleResult = await userManager.AddToRolesAsync(user, createDto.Roles);
            if (!roleResult.Succeeded)
            {
                return roleResult.MapToRamshaErrors();
            }
        }

        await userManager.AddToBaseRoles(user);

        return user.Id.ToString()!;
    }

    protected virtual TUser MapCreate(TCreateDto createDto)
    {
        return new TUser()
        {
            Id = GenerateId(),
            UserName = createDto.Username,
            Email = createDto.Email
        };

    }

    protected virtual TId GenerateId()
    {
        object id = Guid.NewGuid();

        if (typeof(TId) == typeof(Guid))
            return (TId)id;
        if (typeof(TId) == typeof(string))
            return (TId)(object)id.ToString();

        return default;
    }
    protected virtual TUser MapUpdate(TUser user, TUpdateDto updateDto)
    {
        user.PhoneNumber = updateDto.PhoneNumber;
        return user;
    }
    protected virtual TDto ToDto(TUser user)
    {
        return new TDto()
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed
        };
    }

    public virtual async Task<RamshaResult> Delete(TId id)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success)
        {
            return getUser.Errors!;
        }
        var result = await userManager.DeleteAsync(getUser.Data);
        if (!result.Succeeded)
        {
            return result.MapToRamshaErrors();
        }

        return RamshaResult.Ok();
    }

    public virtual async Task<RamshaResult> Update(TId id, TUpdateDto updateDto)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success)
        {
            return getUser.Errors!;
        }
        var user = getUser.Data;
        user = MapUpdate(user, updateDto);
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result.MapToRamshaErrors();
        }

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult<TDto>> Get(TId id)
    {
        var user = await repository.FindAsync(id);
        if (user is null)
        {
            return RamshaError.Create(RamshaErrorsCodes.NOT_FOUND, "no user with this id");
        }

        return ToDto(user);
    }

    public async Task<RamshaResult<List<TDto>>> GetList(TId id)
    {
        var users = await repository.GetListAsync();
        return users.Select(u => ToDto(u)).ToList();
    }



    //role management
    public async Task<RamshaResult> AddToRoleAsync(TId id, string roleName)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.AddToRoleAsync(getUser.Data, roleName);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> RemoveFromRoleAsync(TId id, string roleName)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.RemoveFromRoleAsync(getUser.Data, roleName);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> SetRolesAsync(TId id, IEnumerable<string> roleNames)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.SetRolesAsync(getUser.Data, roleNames);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult<List<string>>> GetRolesAsync(TId id)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var roles = await userManager.GetRolesAsync(getUser.Data);
        return roles.ToList();
    }


    //password management
    public async Task<RamshaResult> ChangePasswordAsync(TId id, string oldPassword, string newPassword)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.ChangePasswordAsync(getUser.Data, oldPassword, newPassword);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> ResetPasswordAsync(TId id, string token, string newPassword)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.ResetPasswordAsync(getUser.Data, token, newPassword);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> SetPasswordAsync(TId id, string newPassword)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.RemovePasswordAsync(getUser.Data);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        result = await userManager.AddPasswordAsync(getUser.Data, newPassword);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }



    //email and username
    public async Task<RamshaResult> SetEmailAsync(TId id, string email)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.SetEmailAsync(getUser.Data, email);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> ConfirmEmailAsync(TId id, string token)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.ConfirmEmailAsync(getUser.Data, token);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public async Task<RamshaResult> SetUserNameAsync(TId id, string username)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.SetUserNameAsync(getUser.Data, username);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }


    //lockout
    public async Task<RamshaResult> SetLockoutAsync(TId id, DateTimeOffset? end)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var result = await userManager.SetLockoutEndDateAsync(getUser.Data, end);
        if (!result.Succeeded) return result.MapToRamshaErrors();

        return RamshaResult.Ok();
    }

    public Task<RamshaResult> UnlockAsync(TId id)
        => SetLockoutAsync(id, null);



    // tokens
    public async Task<RamshaResult<string>> GenerateEmailConfirmationTokenAsync(TId id)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var token = await userManager.GenerateEmailConfirmationTokenAsync(getUser.Data);
        return token;
    }

    public async Task<RamshaResult<string>> GeneratePasswordResetTokenAsync(TId id)
    {
        var getUser = await userManager.GetByIdAsync(id);
        if (!getUser.Success) return getUser.Errors!;

        var token = await userManager.GeneratePasswordResetTokenAsync(getUser.Data);
        return token;
    }

}

