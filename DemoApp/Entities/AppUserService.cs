using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Identity.Application;

namespace DemoApp.Entities;

public class AppUserService(UserManager<AppIdentityUser> userManager)
 : RamshaIdentityUserService<AppIdentityUser, CreateAppUserDto, CreateAppUserDto>(userManager)
{
    protected override AppIdentityUser ToUser(CreateAppUserDto createDto)
    {
        var user = base.ToUser(createDto);
        user.Profile = createDto.Profile;
        return user;
    }
}
