using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityUserToken<TId> : Entity
where TId : IEquatable<TId>
{
    public RamshaIdentityUserToken()
    {

    }
    public RamshaIdentityUserToken(TId userId, string loginProvider, string name, string? value)
    {
        UserId = userId;
        InitializeFrom(loginProvider, name, value);
    }

    public void InitializeFrom(string loginProvider, string name, string? value)
    {
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
    }

    public override object GetId()
    {
        return new { UserId, LoginProvider, Name };
    }

    public virtual TId UserId { get; set; } = default!;
    public virtual string LoginProvider { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual string? Value { get; set; }
}