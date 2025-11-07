using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Domain;

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