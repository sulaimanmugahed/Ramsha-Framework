using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;

namespace DemoApp.Identity;

public class RamshaIdentityUserLogin<TId>
 where TId : IEquatable<TId>
{
    private RamshaIdentityUserLogin()
    {

    }
    public RamshaIdentityUserLogin(TId userId, UserLoginInfo loginInfo)
    {
        UserId = userId;
        LoginProvider = loginInfo.LoginProvider;
        ProviderKey = loginInfo.ProviderKey;
        ProviderDisplayName = loginInfo.ProviderDisplayName;
    }
    public virtual TId UserId { get; set; }

    public virtual string LoginProvider { get; set; }
    public virtual string ProviderKey { get; set; }
    public virtual string? ProviderDisplayName { get; set; }
}

public class RamshaIdentityUserClaim<TId> : Entity<TId>
where TId : IEquatable<TId>
{
    private RamshaIdentityUserClaim()
    {

    }
    public RamshaIdentityUserClaim(TId userId, Claim claim)
    {
        UserId = userId;
        InitializeFromClaim(claim);
    }

    public virtual TId UserId { get; set; }
    public virtual string ClaimType { get; set; }
    public virtual string ClaimValue { get; set; }

    public virtual Claim ToClaim() => new Claim(ClaimType, ClaimValue);

    public virtual void InitializeFromClaim(Claim claim)
    {
        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}

public class RamshaIdentityUserToken<TId>
where TId : IEquatable<TId>
{
    public RamshaIdentityUserToken(TId userId, string loginProvider, string name, string? value)
    {
        UserId = userId;
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
    }

    public virtual TId UserId { get; set; } = default!;
    public virtual string LoginProvider { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual string? Value { get; set; }
}