

using Ramsha.Core;

namespace Ramsha.Identity.Contracts;

public interface IRamshaIdentityUserService<TDto, TCreateDto, TUpdateDto, TId> : IRamshaIdentityUserServiceBase
where TDto : RamshaIdentityUserDto
where TCreateDto : CreateRamshaIdentityUserDto
where TUpdateDto : UpdateRamshaIdentityUserDto
{
    Task<RamshaResult<TDto>> Get(TId id);
    Task<RamshaResult<List<TDto>>> GetList(TId id);
    Task<RamshaResult<string>> Create(TCreateDto createDto);
    Task<RamshaResult> Update(TId id, TUpdateDto updateDto);
    Task<RamshaResult> Delete(TId id);
    Task<RamshaResult> AddToRoleAsync(TId id, string roleName);
    Task<RamshaResult> ChangePasswordAsync(TId id, string oldPassword, string newPassword);
    Task<RamshaResult> ConfirmEmailAsync(TId id, string token);
    Task<RamshaResult<string>> GenerateEmailConfirmationTokenAsync(TId id);
    Task<RamshaResult<string>> GeneratePasswordResetTokenAsync(TId id);
    Task<RamshaResult<List<string>>> GetRolesAsync(TId id);
    Task<RamshaResult> RemoveFromRoleAsync(TId id, string roleName);
    Task<RamshaResult> ResetPasswordAsync(TId id, string token, string newPassword);
    Task<RamshaResult> SetEmailAsync(TId id, string email);
    Task<RamshaResult> SetLockoutAsync(TId id, DateTimeOffset? end);
    Task<RamshaResult> SetPasswordAsync(TId id, string newPassword);
    Task<RamshaResult> SetRolesAsync(TId id, IEnumerable<string> roleNames);
    Task<RamshaResult> SetUserNameAsync(TId id, string username);
    Task<RamshaResult> UnlockAsync(TId id);
}

public interface IRamshaIdentityUserServiceBase;


