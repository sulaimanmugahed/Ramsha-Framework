using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Core;
using Ramsha.Identity.Contracts;

namespace Ramsha.Identity.Api;

[GenericControllerName("users")]
public class RamshaIdentityUserController<TDto, TCreateDto, TUpdateDto, TId>(IRamshaIdentityUserService<TDto, TCreateDto, TUpdateDto, TId> userService) : RamshaApiController
where TCreateDto : CreateRamshaIdentityUserDto
where TUpdateDto : UpdateRamshaIdentityUserDto
where TDto : RamshaIdentityUserDto
{

    [HttpGet("{id}")]
    public async Task<RamshaResult<TDto>> Get(TId id)
    {
        return await UnitOfWork(() => userService.Get(id));
    }

    [HttpGet]
    public async Task<RamshaResult<List<TDto>>> GetList(TId id)
    {
        return await UnitOfWork(() => userService.GetList(id));
    }

    [HttpPost]
    public async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        return await UnitOfWork(() => userService.Create(createDto));
    }

    [HttpPut("{id}")]
    public async Task<RamshaResult> Update(TId id, TUpdateDto updateDto)
    {
        return await UnitOfWork(() => userService.Update(id, updateDto));
    }

    [HttpDelete("{id}")]
    public async Task<RamshaResult> Delete(TId id)
    {
        return await UnitOfWork(() => userService.Delete(id));
    }


    // roles
    [HttpPost("{id}/roles/{roleName}")]
    public async Task<RamshaResult> AddToRole(TId id, string roleName)
    {
        return await UnitOfWork(() => userService.AddToRoleAsync(id, roleName));
    }

    [HttpDelete("{id}/roles/{roleName}")]
    public async Task<RamshaResult> RemoveFromRole(TId id, string roleName)
    {
        return await UnitOfWork(() => userService.RemoveFromRoleAsync(id, roleName));
    }

    [HttpPut("{id}/roles")]
    public async Task<RamshaResult> SetRoles(TId id, List<string> roles)
    {
        return await UnitOfWork(() => userService.SetRolesAsync(id, roles));
    }

    [HttpGet("{id}/roles")]
    public async Task<RamshaResult<List<string>>> GetRoles(TId id)
    {
        return await UnitOfWork(() => userService.GetRolesAsync(id));
    }


    //tokens
    [HttpGet("{id}/token/email-confirmation")]
    public async Task<RamshaResult<string>> GenerateEmailConfirmationToken(TId id)
    {
        return await UnitOfWork(() => userService.GenerateEmailConfirmationTokenAsync(id));
    }

    [HttpGet("{id}/token/password-reset")]
    public async Task<RamshaResult<string>> GeneratePasswordResetToken(TId id)
    {
        return await UnitOfWork(() => userService.GeneratePasswordResetTokenAsync(id));
    }


    //password
    [HttpPost("{id}/password/change")]
    public async Task<RamshaResult> ChangePassword(TId id, string oldPassword, string newPassword)
    {
        return await UnitOfWork(() =>
            userService.ChangePasswordAsync(id, oldPassword, newPassword));
    }

    [HttpPost("{id}/password/set")]
    public async Task<RamshaResult> SetPassword(TId id, string password)
    {
        return await UnitOfWork(() =>
            userService.SetPasswordAsync(id, password));
    }

    [HttpPost("{id}/password/reset")]
    public async Task<RamshaResult> ResetPassword(TId id, string token, string newPassword)
    {
        return await UnitOfWork(() =>
            userService.ResetPasswordAsync(id, token, newPassword));
    }

    //email and user
    [HttpPost("{id}/email/set")]
    public async Task<RamshaResult> SetEmail(TId id, string email)
    {
        return await UnitOfWork(() =>
            userService.SetEmailAsync(id, email));
    }

    [HttpPost("{id}/email/confirm")]
    public async Task<RamshaResult> ConfirmEmail(TId id, string token)
    {
        return await UnitOfWork(() =>
            userService.ConfirmEmailAsync(id, token));
    }

    [HttpPost("{id}/username/set")]
    public async Task<RamshaResult> SetUsername(TId id, string userName)
    {
        return await UnitOfWork(() =>
            userService.SetUserNameAsync(id, userName));
    }

    //token
    [HttpPost("{id}/lockout")]
    public async Task<RamshaResult> SetLockout(TId id, DateTimeOffset? end)
    {
        return await UnitOfWork(() =>
            userService.SetLockoutAsync(id, end));
    }

    [HttpPost("{id}/unlock")]
    public async Task<RamshaResult> Unlock(TId id)
    {
        return await UnitOfWork(() =>
            userService.UnlockAsync(id));
    }

}
