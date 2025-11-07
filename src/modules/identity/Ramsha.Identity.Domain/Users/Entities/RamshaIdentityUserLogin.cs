using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ramsha.Identity.Domain;

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
