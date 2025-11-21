using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Ramsha.Account.Application;
using Ramsha.Identity.Domain;

namespace DemoApp;

public class AppAccountService(RamshaIdentityUserManager<AppIdentityUser> manager)
: RamshaAccountService<AppIdentityUser, AppRegisterDto>(manager)
{
    protected override AppIdentityUser MapCreate(AppRegisterDto registerDto)
    {
        var user = base.MapCreate(registerDto);
        user.Profile = registerDto.Profile;
        return user;
    }
}
