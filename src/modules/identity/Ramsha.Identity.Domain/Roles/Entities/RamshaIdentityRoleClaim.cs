using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityRoleClaim<TId> : Entity<TId>
 where TId : IEquatable<TId>
{
    public RamshaIdentityRoleClaim()
    {

    }
    public RamshaIdentityRoleClaim(TId roleId, Claim claim)
    {
        RoleId = roleId;
        InitializeFromClaim(claim);
    }
    public virtual TId RoleId { get; set; }
    public virtual string ClaimType { get; set; }
    public virtual string ClaimValue { get; set; }

    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType, ClaimValue);
    }

    public virtual void InitializeFromClaim(Claim other)
    {
        ClaimType = other?.Type;
        ClaimValue = other?.Value;
    }
}

