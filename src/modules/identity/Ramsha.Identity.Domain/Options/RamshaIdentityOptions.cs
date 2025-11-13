using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public class RamshaIdentityOptions
{



    public Action<IdentityOptions>? IdentityOptionsAction { get; set; }

    internal List<Action<IdentityBuilder>> ConfigureIdentityActions { get; } = [];

    public RamshaIdentityOptions ConfigureIdentity(Action<IdentityBuilder> action)
    {
        ConfigureIdentityActions.Add(action);
        return this;
    }

 

  

 

}