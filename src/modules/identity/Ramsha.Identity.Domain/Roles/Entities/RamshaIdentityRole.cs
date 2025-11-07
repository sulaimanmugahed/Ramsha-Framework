using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;


public class RamshaIdentityRole : RamshaIdentityRole<Guid>
{
    public RamshaIdentityRole() { }
    public RamshaIdentityRole(string roleName) : this()
    {
        Name = roleName;
    }
}

public class RamshaIdentityRole<TId> : RamshaIdentityRole<TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>>
    where TId : IEquatable<TId>
{
    public RamshaIdentityRole() { }
    public RamshaIdentityRole(string roleName) : this()
    {
        Name = roleName;
    }
}

public class RamshaIdentityRole<TId, TUserRole, TRoleClaim> : AggregateRoot<TId>
    where TId : IEquatable<TId>
    where TUserRole : RamshaIdentityUserRole<TId>
    where TRoleClaim : RamshaIdentityRoleClaim<TId>
{

    public virtual ICollection<TUserRole> Users { get; } = new List<TUserRole>();
    public virtual ICollection<TRoleClaim> Claims { get; } = new List<TRoleClaim>();
    public virtual TId Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string? NormalizedName { get; set; }


    public RamshaIdentityRole() { }

    public RamshaIdentityRole(string roleName) : this()
    {
        Name = roleName;
    }

    public void AddClaim(Claim claim)
    {
        Claims.Add((TRoleClaim)new RamshaIdentityRoleClaim<TId>(Id, claim));
    }

    public void RemoveClaim(Claim claim)
    {
        var claimToRemove = FindClaim(claim);
        if (claimToRemove is not null)
        {
            Claims.Remove(claimToRemove);
        }
    }

    public TRoleClaim? FindClaim(Claim claim)
    {
        return Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
    }
}
