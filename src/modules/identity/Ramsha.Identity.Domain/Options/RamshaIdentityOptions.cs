using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityOptions
{
    public Type UserType { get; private set; } = typeof(RamshaIdentityUser);
    public Type RoleType { get; private set; } = typeof(RamshaIdentityRole);
    public Type KeyType { get; private set; }
    public Type UserRoleType { get; private set; } = typeof(RamshaIdentityUserRole<Guid>);
    public Type RoleClaimType { get; private set; } = typeof(RamshaIdentityRoleClaim<Guid>);
    public Type UserClaimType { get; private set; } = typeof(RamshaIdentityUserClaim<Guid>);
    public Type UserLoginType { get; private set; } = typeof(RamshaIdentityUserLogin<Guid>);
    public Type UserTokenType { get; private set; } = typeof(RamshaIdentityUserToken<Guid>);



    public Action<IdentityOptions>? IdentityOptionsAction { get; set; }

    internal List<Action<IdentityBuilder>> ConfigureIdentityActions { get; } = [];

    public RamshaIdentityOptions ConfigureIdentity(Action<IdentityBuilder> action)
    {
        ConfigureIdentityActions.Add(action);
        return this;
    }

    public RamshaIdentityOptions IdentityTypes<TUser>()
    where TUser : RamshaIdentityUser
    {
        UserType = typeof(TUser);
        KeyType = EntityHelper.FindPrimaryKeyType(UserType) ?? typeof(Guid);
        return this;
    }

    public RamshaIdentityOptions IdentityTypes<TUser, TRole>()
    where TUser : RamshaIdentityUser
    where TRole : RamshaIdentityRole
    {
        UserType = typeof(TUser);
        RoleType = typeof(TRole);
        return this;
    }

}