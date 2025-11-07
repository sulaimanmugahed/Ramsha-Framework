using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityUserRole<TId> where TId : IEquatable<TId>
{
    public RamshaIdentityUserRole(TId userId, TId roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public virtual TId UserId { get; set; }
    public virtual TId RoleId { get; set; }
}
