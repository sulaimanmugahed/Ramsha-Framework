using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityUser : RamshaIdentityUser<Guid>
{
    protected RamshaIdentityUser()
    {

    }
    public RamshaIdentityUser(Guid id, string username) : base(id, username)
    {

    }

}

public class RamshaIdentityUser<TId>
 : RamshaIdentityUser<TId, RamshaIdentityUserClaim<TId>, RamshaIdentityUserRole<TId>, RamshaIdentityUserLogin<TId>, RamshaIdentityUserToken<TId>>
 where TId : IEquatable<TId>

{
    protected RamshaIdentityUser()
    {

    }
    public RamshaIdentityUser(TId id, string username) : base(id, username)
    {

    }

}

public class RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 : RamshaIdentityUserBase<TId>
 where TId : IEquatable<TId>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>

{

    protected RamshaIdentityUser() { }
    public RamshaIdentityUser(TId id, string userName) : this()
    {
        Id = id;
        UserName = userName;
    }
   
    public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();
    public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();
    public virtual ICollection<TUserLogin> Logins { get; } = new List<TUserLogin>();
    public virtual ICollection<TUserToken> Tokens { get; } = new List<TUserToken>();








    //
    public virtual void AddRole(TId roleId)
    {
        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add((TUserRole)new RamshaIdentityUserRole<TId>(Id, roleId));
    }

    public virtual void RemoveRole(TId roleId)
    {
        if (!IsInRole(roleId))
        {
            return;
        }

        var role = Roles.FirstOrDefault(r => r.RoleId.Equals(roleId));
        if (role is not null)
        {
            Roles.Remove(role);
        }
    }

    public virtual bool IsInRole(TId roleId)
    {
        return Roles.Any(r => r.RoleId.Equals(roleId));
    }

    public virtual RamshaIdentityUserClaim<TId>? FindClaim(Claim claim)
    {
        return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    public virtual void ReplaceClaim(Claim claim, Claim newClaim)
    {
        var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
        foreach (var userClaim in userClaims)
        {
            userClaim.InitializeFromClaim(newClaim);
        }
    }

    public virtual void RemoveClaims(IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            RemoveClaim(claim);
        }
    }

    public virtual void RemoveClaim(Claim claim)
    {
        var claimToRemove = Claims.FirstOrDefault(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
        if (claimToRemove is not null)
        {
            Claims.Remove(claimToRemove);
        }
    }

    public virtual void RemoveLogin(string loginProvider, string providerKey)
    {
        var login = Logins.FirstOrDefault(userLogin =>
              userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);

        if (login is not null)
        {
            Logins.Remove(login);
        }
    }

    public virtual RamshaIdentityUserToken<TId>? FindToken(string loginProvider, string name)
    {
        return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
    }


    public virtual void RemoveToken(string loginProvider, string name)
    {
        var token = Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);

        if (token is not null)
        {
            Tokens.Remove(token);
        }
    }



    public virtual void AddClaim(Claim claim)
    {
        if (!Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
        {
            Claims.Add((TUserClaim)new RamshaIdentityUserClaim<TId>(Id, claim));
        }
    }
    public virtual void AddClaims(IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            AddClaim(claim);
        }
    }


    public virtual void AddLogin(UserLoginInfo login)
    {
        Logins.Add((TUserLogin)new RamshaIdentityUserLogin<TId>(Id, login));
    }

    public virtual void AddToken(TUserToken token)
    {
        Tokens.Add(token);
    }


    public override string ToString()
    {
        return UserName;
    }

}

public abstract class RamshaIdentityUserBase<TId>:AggregateRoot<TId>, IAudit, ISoftDelete
where TId:IEquatable<TId>
{
    public virtual string UserName { get; set; }
    public virtual string? Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual string? NormalizedEmail { get; set; }
    public virtual string? NormalizedUserName { get; set; }
    public virtual string PasswordHash { get; set; }
    public virtual string? SecurityStamp { get; set; }
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public virtual string? PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }
    public virtual bool TwoFactorEnabled { get; set; }
    public virtual DateTimeOffset? LockoutEnd { get; set; }
    public virtual bool LockoutEnabled { get; set; }
    public virtual int AccessFailedCount { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletionDate { get; set; }
}
