using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Identity.Application;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Domain;

namespace DemoApp.Entities;

public class AppUserService(RamshaIdentityUserManager<AppIdentityUser> userManager, IIdentityUserRepository<AppIdentityUser, Guid> repository)
 : RamshaIdentityUserService<AppIdentityUser, RamshaIdentityUserDto, CreateAppUserDto, UpdateAppUserDto>(userManager, repository)
{
    protected override AppIdentityUser MapCreate(CreateAppUserDto createDto)
    {
        var user = base.MapCreate(createDto);
        user.Profile = createDto.Profile;
        return user;
    }

    protected override AppIdentityUser MapUpdate(AppIdentityUser user, UpdateAppUserDto updateDto)
    {
        base.MapUpdate(user, updateDto);
        if (updateDto.Profile is not null)
        {
            user.Profile = updateDto.Profile;
        }

        return user;
    }
}
