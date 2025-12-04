using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Common.Domain;


namespace Ramsha.Identity.Domain;

public class RamshaIdentityUserLogin<TId> : Entity
 where TId : IEquatable<TId>
{
    public RamshaIdentityUserLogin()
    {

    }
    public RamshaIdentityUserLogin(TId userId, string loginProvider, string providerKey, string? providerDisplayName = null)
    {
        UserId = userId;
        InitializeFrom(loginProvider, providerKey, providerDisplayName);
    }
    public virtual TId UserId { get; set; }

    public void InitializeFrom(string loginProvider, string providerKey, string? providerDisplayName = null)
    {
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
    }

    public override object GetId()
    {
        return new { LoginProvider, ProviderKey };
    }

    public virtual string LoginProvider { get; set; }
    public virtual string ProviderKey { get; set; }
    public virtual string? ProviderDisplayName { get; set; }
}
