using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityOptions
{
    public Type UserType { get; private set; } = typeof(RamshaIdentityUser<Guid>);
    public Type RoleType { get; private set; } = typeof(RamshaIdentityRole<Guid>);
    public Type KeyType { get; private set; } = typeof(Guid);
    public Type UserRoleType { get; private set; }
    public Type RoleClaimType { get; private set; }
    public Type UserClaimType { get; private set; }
    public Type UserLoginType { get; private set; }
    public Type UserTokenType { get; private set; }

    public IdentityOptions IdentityOptions { get; private set; } = new IdentityOptions();

    public RamshaIdentityOptions WithUserType<TUser>() where TUser : class
    {
        UserType = typeof(TUser);
        return this;
    }

    public RamshaIdentityOptions WithRoleType<TRole>() where TRole : class
    {
        RoleType = typeof(TRole);
        return this;
    }

    public RamshaIdentityOptions WithKeyType<TKey>() where TKey : IEquatable<TKey>
    {
        KeyType = typeof(TKey);
        return this;
    }

    public RamshaIdentityOptions WithUserRoleType<TUserRole>()
    {
        UserRoleType = typeof(TUserRole);
        return this;
    }

    public RamshaIdentityOptions WithRoleClaimType<TRoleClaim>()
    {
        RoleClaimType = typeof(TRoleClaim);
        return this;
    }

    public RamshaIdentityOptions WithUserClaimType<TUserClaim>()
    {
        UserClaimType = typeof(TUserClaim);
        return this;
    }

    public RamshaIdentityOptions WithUserLoginType<TUserLogin>()
    {
        UserLoginType = typeof(TUserLogin);
        return this;
    }

    public RamshaIdentityOptions WithUserTokenType<TUserToken>()
    {
        UserTokenType = typeof(TUserToken);
        return this;
    }

    public RamshaIdentityOptions ConfigureIdentity(Action<IdentityOptions> configure)
    {
        configure(IdentityOptions);
        return this;
    }
}