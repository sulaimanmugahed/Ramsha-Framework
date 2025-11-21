using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ramsha.Common.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityUserClaim<TId> : Entity<TId>
where TId : IEquatable<TId>
{
    public RamshaIdentityUserClaim()
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
