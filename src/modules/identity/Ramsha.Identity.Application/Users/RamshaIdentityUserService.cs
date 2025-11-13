using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Core;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Core;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Application;

public class RamshaIdentityUserService<TUser, TCreateDto, TUpdateDto>(UserManager<TUser> userManager) : RamshaIdentityUserService<TUser, RamshaIdentityRole, Guid, RamshaIdentityUserRole<Guid>, RamshaIdentityRoleClaim<Guid>, RamshaIdentityUserClaim<Guid>, RamshaIdentityUserLogin<Guid>, RamshaIdentityUserToken<Guid>, TCreateDto, TUpdateDto>(userManager)
where TUser : RamshaIdentityUser, new()
where TCreateDto : CreateRamshaIdentityUserDto

{

}


public class RamshaIdentityUserService<TUser, TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TCreateDto, TUpdateDto>(UserManager<TUser> userManager) : IRamshaIdentityUserService<TCreateDto, TUpdateDto, TId>

 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>, new()
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
    where TCreateDto : CreateRamshaIdentityUserDto

{


    public virtual async Task<RamshaResult<string>> Create(TCreateDto createDto)
    {
        var user = ToUser(createDto);
        var identityResult = await userManager.CreateAsync(user, createDto.Password);
        if (!identityResult.Succeeded)
        {
            var result = RamshaResult<string>.Failure();
            foreach (var error in identityResult.Errors)
            {
                result.AddError(RamshaError.Create(RamshaErrorsCodes.ValidationErrorCode, error.Description));
            }
            return result;
        }

        return user.Id.ToString()!;
    }

    protected virtual TUser ToUser(TCreateDto createDto)
    {
        return new TUser()
        {
            UserName = createDto.Username,
            Email = createDto.Email
        };

    }

    public virtual async Task<RamshaResult> Delete(TId id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return RamshaError.Create(RamshaErrorsCodes.EntityNotFoundErrorCode, "no user found with this id");
        }
        await userManager.DeleteAsync(user);
        return RamshaResult.Ok();
    }

    public virtual Task<RamshaResult> Update(TUpdateDto updateDto)
    {
        throw new NotImplementedException();
    }
}
